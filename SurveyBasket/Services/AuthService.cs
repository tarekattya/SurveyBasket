
using SurveyBasket.Authentication;
using SurveyBasket.Errors;
using System.Security.Cryptography;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager , IJWTprovider jWTprovider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJWTprovider _jWTprovider = jWTprovider;
        private readonly int _refreshTokenExpireDays = 14; 

        public async Task<Result<AuthResponse>> GetTokenAsync(string Email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null) 
                return Result.Failure<AuthResponse>(AuthErrors.InvalidUserCredentials);
            var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if(!IsValidPassword)
                return Result.Failure<AuthResponse>(AuthErrors.InvalidUserCredentials);
            


            (string Token , int ExpireInMinutes) =  _jWTprovider.GenerateToken(user);

            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpireDate = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpireOn = refreshTokenExpireDate,
                CreatedOn = DateTime.UtcNow
            });

             await _userManager.UpdateAsync(user);

            var response =new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName,Token , ExpireInMinutes * 60 , refreshToken , refreshTokenExpireDate);
            return Result.Success(response);
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

            var response =  new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, ExpireInMinutes * 60, NewrefreshToken, refreshTokenExpireDate);

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
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

       
    }
}
