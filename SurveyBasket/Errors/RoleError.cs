namespace SurveyBasket.Errors
{
    public static class RoleError
    {
        public static Error RoleNotFound = new("Role.NotFound", "Role Not found", StatusCodes.Status404NotFound);
        public static Error RoleAlreadyExists = new("Role.RoleAlreadyExists", "Role Already Exists", StatusCodes.Status409Conflict);
        public static Error InvalidPermission = new("Role.InvalidPermission", "Invalid Permission", StatusCodes.Status400BadRequest);
        public static Error RoleCreationFailed = new("Role.RoleCreationFailed", "Role Creation Failed", StatusCodes.Status400BadRequest);



            
    
    }
}
