namespace SurveyBasket.Authentication
{
    public interface IJWTprovider
    {
        (string token , int Expirein) GenerateToken(ApplicationUser user);
        String ValidateToken(string token);
    }
}
