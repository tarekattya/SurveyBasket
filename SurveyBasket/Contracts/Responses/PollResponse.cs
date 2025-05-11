namespace SurveyBasket.Contracts.Responses
{
    public record PollResponse(int id, string Title, string Summary , bool IsPublished, DateTime StartsAt, DateTime EndsAt)
    {
        //public int Id { get; set; }

        //public string Title { get; set; } = string.Empty;
        //public string Description { get; set; } = string.Empty;

        //public static implicit operator PollResponse(Poll poll)
        //{
        //    return new()
        //    {
        //        Id = poll.Id,
        //        Title = poll.Title,
        //        Description = poll.Description,
        //    };
        //}
    }
}
