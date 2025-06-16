namespace SurveyBasket.Contracts.Account
{
    public record UpdateProfileRequest(string FirstName,
             string LastName,
             IFormFile? ProfileImage);
    


           
        
    
}
