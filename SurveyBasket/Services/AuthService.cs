
using SurveyBasket.Authentication;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager , IJWTprovider jWTprovider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJWTprovider _jWTprovider = jWTprovider;

        public async Task<AuthResponse?> GetTokenAsync(string Email, string password, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null) 
                return null;
            var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if(!IsValidPassword)
                return null;


            (string Token , int ExpireIn) =  _jWTprovider.GenerateToken(user);

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName,Token , ExpireIn*60  );
        }
    }
}
