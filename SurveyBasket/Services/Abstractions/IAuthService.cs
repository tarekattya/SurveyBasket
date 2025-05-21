﻿

namespace SurveyBasket.Services.Abstractions
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> GetTokenAsync(string Email, string password, CancellationToken cancellationToken);
        Task<Result<AuthResponse?>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
        Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
    }
}
