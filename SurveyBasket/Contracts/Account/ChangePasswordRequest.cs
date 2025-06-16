namespace SurveyBasket.Contracts.Account
{
    public record ChangePasswordRequest
        (string oldpassword,
         string newpassword,
         string confirmpassword);
}
