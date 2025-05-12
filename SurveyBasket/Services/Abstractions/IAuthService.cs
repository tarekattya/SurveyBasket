
namespace SurveyBasket.Services.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResponse?> GetTokenAsync(string Email, string password, CancellationToken cancellationToken);
    }
}
