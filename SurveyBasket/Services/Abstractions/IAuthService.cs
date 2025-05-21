

namespace SurveyBasket.Services.Abstractions
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string Email, string password, CancellationToken cancellationToken);
        Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
        Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
    }
}
