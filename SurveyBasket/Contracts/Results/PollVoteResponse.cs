namespace SurveyBasket.Contracts.Results
{
    public record PollVoteResponse(
        string title,
        string summary,
        DateOnly StartsAt,
        DateOnly EndsAt,
        IEnumerable<VoteResponse> Votes
        );


}
