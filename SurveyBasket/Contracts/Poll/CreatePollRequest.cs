using System.Runtime.CompilerServices;

namespace SurveyBasket.Contracts.Poll
{
    public record CreatePollRequest(string Title, string Summary, DateTime StartsAt, DateTime EndsAt) { }




    //public static  implicit operator Poll(CreatePollRequest request)
    //{
    //    return new()
    //    {
    //        Title = request.Title,
    //        Description = request.Description,
    //    };
    //}



}
