namespace SurveyBasket.Errors
{
    public class PollError
    {
        public static Error PollNotFound => new("Poll Not Found", $"Poll with id not found");

    }
}
