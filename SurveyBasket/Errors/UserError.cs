namespace SurveyBasket.Errors
{
    public static class UserError
    {
        public static readonly Error InvalidUserCredentials = new("InvalidUserCredentials", "Invalid Email/Password");
    }
}
