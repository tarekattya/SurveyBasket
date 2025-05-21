namespace SurveyBasket.Contracts.Poll
{
    public record PollResponse(
        int id, 
        string Title,
        string Summary,
        bool IsPublished,
        DateTime StartsAt, 
        DateTime EndsAt
        );
 }

