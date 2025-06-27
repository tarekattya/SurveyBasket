namespace SurveyBasket.Contracts.Roles
{
    public record RoleDeatilsResponse(string id, string name, bool IsDeleted, IEnumerable<string> Permissions);


}
