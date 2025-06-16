using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.Contracts.Account;
using System.Text;

namespace SurveyBasket.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<GetProfileResponse>> GetProfileAsync(string userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId)
                .ProjectToType<GetProfileResponse>()
                .SingleAsync();

            return Result.Success<GetProfileResponse>(user);
        }

        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            using var ms = new MemoryStream();
            await request.ProfileImage.CopyToAsync(ms);
            var imageBytes = ms.ToArray();
            var contentType = request.ProfileImage.ContentType;

            await _userManager.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(u=>u.FirstName , request.FirstName)
                    .SetProperty(u=>u.LastName , request.LastName)
                    .SetProperty(u => u.ProfileImage, imageBytes)
                    .SetProperty(u => u.ProfileImageContentType, contentType));

            return Result.Success();
        }


        


        public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
           
            var result = await _userManager.ChangePasswordAsync(user!, request.oldpassword, request.newpassword);

            if (result.Succeeded)
                return Result.Success();

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code , error.Description , StatusCodes.Status400BadRequest));

        }
    }
}
