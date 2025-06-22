public static class Permissions
{
    public static string Type { get; } = "permissions";

    // Polls Permissions
    public const string Polls_Read = "polls:read";
    public const string Polls_Add = "polls:add";
    public const string Polls_Update = "polls:update";
    public const string Polls_Delete = "polls:delete";

    // Questions Permissions
    public const string Questions_Read = "questions:read";
    public const string Questions_Add = "questions:add";
    public const string Questions_Update = "questions:update";

    // Users Permissions
    public const string Users_Read = "users:read";
    public const string Users_Add = "users:add";
    public const string Users_Update = "users:update";

    // Roles Permissions
    public const string Roles_Read = "roles:read";
    public const string Roles_Add = "roles:add";
    public const string Roles_Update = "roles:update";

    // Results Permissions
    public const string Results_Read = "results:read";

    public static IList<string?> GetAllPermission() =>
        typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
}
