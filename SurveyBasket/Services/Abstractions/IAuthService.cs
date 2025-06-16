

using SurveyBasket.Contracts.Account;

namespace SurveyBasket.Services.Abstractions
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string Email, string password, CancellationToken cancellationToken);
        Task<Result<AuthResponse?>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
        Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
        Task<Result> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken);
        Task<Result> ResendConfirmEmailAsync(ResendConfirmationCodeRequest request, CancellationToken cancellationToken);

        Task<Result> ResendResetCodeAsync(string email);

        Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
