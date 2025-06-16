namespace SurveyBasket.Contracts.Account
{
    public record ResetPasswordRequest(
        string Email,
        string Token,
        string NewPassword


        );
    
    
}
