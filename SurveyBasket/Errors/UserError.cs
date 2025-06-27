namespace SurveyBasket.Errors
{
    public static class UserError
    {
        public static Error UserNotFound = new("UserNotFound", "The specified user was not found.", StatusCodes.Status404NotFound);
        public static Error EmailAlreadyExists = new("EmailAlreadyExists", "Email Already Exists", StatusCodes.Status409Conflict);
        public static Error InvalidRole = new("InvalidRole", "Invalid Role", StatusCodes.Status409Conflict);



    }
}
