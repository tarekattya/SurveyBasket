namespace SurveyBasket.Contracts.Account
{
    public record GetProfileResponse
    (

        string FirstName,
        string LastName, 
        string Email ,
        string? ProfileImage,
        string? ProfileImageContentType
    );
}
