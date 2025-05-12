namespace SurveyBasket.Contracts.Authentication
{
    public record AuthResponse(string UserId, string? Email ,string FirstName, string LastName,string Token, int ExpireIn);
    
}
