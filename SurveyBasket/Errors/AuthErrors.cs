namespace SurveyBasket.Errors
{
    public static class AuthErrors
    {
        public static Error InvalidTokens = new("Invalid Tokens", "The provided tokens are invalid." , StatusCodes.Status401Unauthorized);

        public static Error DisabledUser = new("Disabled User", "Disabled User , contact your administrator", StatusCodes.Status401Unauthorized);
        public static Error LockedUser = new("Locked User", "Locked User , contact your administrator", StatusCodes.Status401Unauthorized);

        public static Error InvalidUserCredentials = new("InvalidUserCredentials", "Invalid Email/Password",StatusCodes.Status401Unauthorized);


        public static Error DublicateEmail = new("DublicateEmail", "This Email Alreadr have Account", StatusCodes.Status409Conflict);

        public static Error NotConfirmedEmail = new("NotConfirmedEmail", "Not Confirmed Email", StatusCodes.Status401Unauthorized);
        public static Error DublicateConfirmedEmail = new("DublicateConfirmedEmail", "Dublicate Confirmed Email", StatusCodes.Status409Conflict);
        public static Error InvalidConfirmCode = new("InvalidConfirmCode", "Invalid Confirme Code", StatusCodes.Status400BadRequest);

    }
}
