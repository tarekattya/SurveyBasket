
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Authentication;
using SurveyBasket.Contracts.Account;
using SurveyBasket.Errors;
using SurveyBasket.Helpers;
using SurveyBasket.Presistence.DbContextt;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using ResetPasswordRequest = SurveyBasket.Contracts.Account.ResetPasswordRequest;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IJWTprovider jWTprovider, ILogger<IAuthService> logger, IHttpContextAccessor contextAccessor 
        , IEmailSender emailSender
        ,ApplicationDbContext context) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJWTprovider _jWTprovider = jWTprovider;
        private readonly ILogger<IAuthService> _logger = logger;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly ApplicationDbContext _context = context;
        private readonly int _refreshTokenExpireDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string Email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidUserCredentials);

            if(user.IsDisable)
                return Result.Failure<AuthResponse>(AuthErrors.DisabledUser);

            if(user.LockoutEnd > DateTime.UtcNow)
                return Result.Failure<AuthResponse>(AuthErrors.LockedUser);


            var result = await _signInManager.PasswordSignInAsync(user, password, false, true);
            if (result.Succeeded)
            {
               var (userRoles , Permission) =  await GetUserClaimsAsync(user, cancellationToken);
               var ( Token,  ExpireInMinutes) = _jWTprovider.GenerateToken(user , userRoles , Permission);

                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpireDate = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpireOn = refreshTokenExpireDate,
                    CreatedOn = DateTime.UtcNow
                });

                await _userManager.UpdateAsync(user);

                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, Token, ExpireInMinutes * 60, refreshToken, refreshTokenExpireDate);
                return Result.Success(response);
            }


            var error  = result.IsNotAllowed ? AuthErrors.NotConfirmedEmail 
                : result.IsLockedOut ? AuthErrors.LockedUser :
                AuthErrors.InvalidUserCredentials;

            return Result.Failure<AuthResponse>(error);


        }


        public async Task<Result<AuthResponse?>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
        {
            var userid = _jWTprovider.ValidateToken(token);
            if (userid is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidTokens)!;

            var user = await _userManager.FindByIdAsync(userid);
            if (user is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidTokens)!;

            if (user.IsDisable)
                return Result.Failure<AuthResponse?>(AuthErrors.DisabledUser);

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidTokens)!;
            userRefreshToken.RevokedOn = DateTime.UtcNow;
            var (userRoles, Permission) = await GetUserClaimsAsync(user, cancellationToken);
            (string newToken, int ExpireInMinutes) = _jWTprovider.GenerateToken(user, userRoles, Permission);

            var NewrefreshToken = GenerateRefreshToken();
            var refreshTokenExpireDate = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = NewrefreshToken,
                ExpireOn = refreshTokenExpireDate,
                CreatedOn = DateTime.UtcNow
            });

            await _userManager.UpdateAsync(user);

            var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, ExpireInMinutes * 60, NewrefreshToken, refreshTokenExpireDate);

            return Result.Success(response)!;
        }
        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
        {
            var userid = _jWTprovider.ValidateToken(token);
            if (userid is null)
                return Result.Failure(AuthErrors.InvalidUserCredentials);
            var user = await _userManager.FindByIdAsync(userid);
            if (user is null)
                return Result.Failure(AuthErrors.InvalidUserCredentials);
            var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null)
                return Result.Failure(AuthErrors.InvalidUserCredentials);
            userRefreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return Result.Success();
        }


        public async Task<Result> RegisterAsync(Contracts.Authentication    .RegisterRequest request, CancellationToken cancellationToken)
        {
            var emailExists = await _userManager.Users.AnyAsync(U => U.Email == request.Email, cancellationToken);
            if (emailExists)
                return Result.Failure<AuthResponse>(AuthErrors.DublicateEmail);

            var user = request.Adapt<ApplicationUser>();

            var Create = await _userManager.CreateAsync(user, request.Password);

            if (Create.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation("Confirmation Code {code}", code);
                await SendConfirmationEmail(user, code);



                return Result.Success(code);
            } 
            var error = Create.Errors.First();
            return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null)
                return Result.Failure(AuthErrors.InvalidUserCredentials);

            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.DublicateConfirmedEmail);

            var code = request.Token;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(AuthErrors.InvalidConfirmCode);

            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleDefault.Member); 
                return Result.Success();
            }
            var error = result.Errors.First();

            return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }


        public async Task<Result> ResendConfirmEmailAsync(ResendConfirmationCodeRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return Result.Success();
            if (user.EmailConfirmed)
                return Result.Failure(AuthErrors.DublicateConfirmedEmail);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confirmation Code {code}", code);
            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        public async Task<Result> ResendResetCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result.Success();
            if (!user.EmailConfirmed)
                return Result.Failure(AuthErrors.NotConfirmedEmail);
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confirmation Code {code}", code);
            await SendResetCode(user, code);

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (!user!.EmailConfirmed || user is null)
                return Result.Failure(AuthErrors.InvalidTokens);

            IdentityResult result;

            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
                result = await _userManager.ResetPasswordAsync(user!, code, request.NewPassword);
            }
            catch (FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }


            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task SendConfirmationEmail(ApplicationUser user , string code)
        {
            var origin = _contextAccessor.HttpContext?.Request.Headers.Origin;

            var emailbody = EmailBodyCreatero.GenerateBodyEmail("EmailConfirmation",

                TempModel: new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName},
                    {"{{action_url}}", $"{origin}/auth/ConfirmationEmail?UserId={user.Id}&code={code}" }

                });
           BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Confirmation Email", emailbody));

            await Task.CompletedTask;
        }


        private async Task<(IEnumerable<string> UserRoles , IEnumerable<string> UserPermission)> GetUserClaimsAsync(ApplicationUser user , CancellationToken cancellationToken){

            var UserRoles = await _userManager.GetRolesAsync(user);

            var UserPermission = await (from r in _context.Roles
                                        join p in _context.RoleClaims
                                        on r.Id equals p.RoleId
                                        where UserRoles.Contains(r.Name!)
                                        select p.ClaimValue


                                        ).Distinct().ToListAsync(cancellationToken);


            return (UserRoles, UserPermission);

                                       


            }
        private async Task SendResetCode(ApplicationUser user, string code)
        {
            var origin = _contextAccessor.HttpContext?.Request.Headers.Origin;

            var emailbody = EmailBodyCreatero.GenerateBodyEmail("ForgetPassword",

                TempModel: new Dictionary<string, string>
                {
                    { "{{name}}", user.FirstName},
                    {"{{action_url}}", $"{origin}/auth/Reset?UserId={user.Id}&code={code}" }

                });
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Reset Code", emailbody));

            await Task.CompletedTask;
        }
    }
}
