using System.Security.Claims;

namespace SurveyBasket.Excetension
{
    public static class UserExcetension
    {
        public static string? GetUserId(this ClaimsPrincipal user)=>
        
             user.FindFirstValue(ClaimTypes.NameIdentifier);
        
    }
}
