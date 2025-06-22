namespace SurveyBasket.Authentication
{
    public interface IJWTprovider
    {
        (string token , int Expirein) GenerateToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permission);
        String ValidateToken(string token);
    }
}
