
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Authentication;
using SurveyBasket.Errors;
using SurveyBasket.Helpers;
using System.Security.Cryptography;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
        IJWTprovider jWTprovider, ILogger<IAuthService> logger, IHttpContextAccessor contextAccessor , IEmailSender emailSender) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJWTprovider _jWTprovider = jWTprovider;
        private readonly ILogger<IAuthService> _logger = logger;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly int _refreshTokenExpireDays = 14;

        public async Task<Result<AuthResponse>> GetTokenAsync(string Email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidUserCredentials);
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (result.Succeeded)
            {
                (string Token, int ExpireInMinutes) = _jWTprovider.GenerateToken(user);

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


            return Result.Failure<AuthResponse>(result.IsNotAllowed ? AuthErrors.NotConfirmedEmail : AuthErrors.InvalidUserCredentials);


        }


        public async Task<Result<AuthResponse?>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
        {
            var userid = _jWTprovider.ValidateToken(token);
            if (userid is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidTokens)!;

            var user = await _userManager.FindByIdAsync(userid);
            if (user is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidTokens)!;

            var userRefreshToken = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidTokens)!;
            userRefreshToken.RevokedOn = DateTime.UtcNow;
            (string newToken, int ExpireInMinutes) = _jWTprovider.GenerateToken(user);

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


        public async Task<Result> RegisterAsync(Contracts.Authentication.RegisterRequest request, CancellationToken cancellationToken)
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
                await SendEmail(user, code);

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
            await SendEmail(user, code);

            return Result.Success();
        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task SendEmail(ApplicationUser user , string code)
        {
            var origin = _contextAccessor.HttpContext?.Request.Headers.Origin;

            var emailbody = EmailBodyCreatero.GenerateBodyEmail("EmailConfirmation",

                TempModel: new Dictionary<string, string>
                {
                    { "{{Name}}", user.FirstName},
                    {"{{action_url}}", $"{origin}/auth/ConfirmationEmail?UserId={user.Id}&code={code}" }

                });
           await _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Confirmation Email", emailbody);

        }
    }
}
