namespace SurveyBasket.Errors
{
    public static class AuthErrors
    {
        public static Error InvalidTokens => new("Invalid Tokens", "The provided tokens are invalid.");

        public static Error InvalidUserCredentials => new("InvalidUserCredentials", "Invalid Email/Password");

    }
}
