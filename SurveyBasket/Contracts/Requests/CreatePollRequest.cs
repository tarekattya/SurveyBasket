using System.Runtime.CompilerServices;

namespace SurveyBasket.Contracts.Requests
{
    public record CreatePollRequest(string Title , string Summary, bool IsPublished, DateTime StartsAt, DateTime EndsAt) { }
    
       


        //public static  implicit operator Poll(CreatePollRequest request)
        //{
        //    return new()
        //    {
        //        Title = request.Title,
        //        Description = request.Description,
        //    };
        //}
    


}
