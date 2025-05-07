using System.Runtime.CompilerServices;

namespace SurveyBasket.Contracts.Requests
{
    public record CreatePollRequest(string Title , string Description) { }
    
       


        //public static  implicit operator Poll(CreatePollRequest request)
        //{
        //    return new()
        //    {
        //        Title = request.Title,
        //        Description = request.Description,
        //    };
        //}
    


}
