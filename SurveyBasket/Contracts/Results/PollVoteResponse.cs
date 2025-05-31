namespace SurveyBasket.Contracts.Results
{
    public record PollVoteResponse(
        string title,
        string summary,
        DateTime StartsAt,
        DateTime EndsAt,
        IEnumerable<VoteResponse> Votes
        );


}
