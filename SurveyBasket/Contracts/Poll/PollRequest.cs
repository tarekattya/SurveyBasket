using System.Runtime.CompilerServices;

namespace SurveyBasket.Contracts.Poll
{
    public record PollRequest(string Title, string Summary, DateOnly StartsAt, DateOnly EndsAt) { }


    

    //public static  implicit operator Poll(CreatePollRequest request)
    //{
    //    return new()
    //    {
    //        Title = request.Title,
    //        Description = request.Description,
    //    };
    //}



}
