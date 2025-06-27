namespace SurveyBasket.Contracts.User
{
    public record UserResponse(
        string id,
        string FirstName,
        string LastName,
        string Email,
        bool isDisable,
        IEnumerable<string> Roles
        
        );
    
}
