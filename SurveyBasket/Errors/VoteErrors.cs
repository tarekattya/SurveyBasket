namespace SurveyBasket.Errors
{
    public class VoteErrors
    {
        public static Error DublicateVotes => new("Vote.DublicateVotes", $"The user has voted this poll before" , StatusCodes.Status409Conflict);
        public static Error InvalidQuestions => new("Vote.InvalidQuestions", $"Question not correct at this Poll", StatusCodes.Status400BadRequest);

    }
}
