using SurveyBasket.Contracts.Account;

namespace SurveyBasket.Services.Abstractions
{
    public interface IAccountService
    {

        Task<Result<GetProfileResponse>> GetProfileAsync(string userId);
        Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);

        Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
    }
}
