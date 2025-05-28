namespace SurveyBasket.Errors
{
    public static class AuthErrors
    {
        public static Error InvalidTokens => new("Invalid Tokens", "The provided tokens are invalid." , StatusCodes.Status401Unauthorized);

        public static Error InvalidUserCredentials => new("InvalidUserCredentials", "Invalid Email/Password",StatusCodes.Status401Unauthorized);

    }
}
