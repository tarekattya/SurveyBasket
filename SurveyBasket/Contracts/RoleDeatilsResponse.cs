namespace SurveyBasket.Contracts
{
    public record RoleDeatilsResponse(string id , string name , bool IsDeleted , IEnumerable<string> Permissions);
    
    
}
