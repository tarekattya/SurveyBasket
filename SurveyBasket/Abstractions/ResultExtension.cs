using System.Reflection;

namespace SurveyBasket.Abstractions
{
    public static class ResultExtension
    {

        public static ObjectResult ToProblem(this Result result)
        {

            if (result.IsSuccess)
                throw new InvalidOperationException("cannot convert succes result to problem");


            var problem = Results.Problem();
            var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

            problemDetails!.Extensions = new Dictionary<string, object?>()
               {{
                       "errors",
                         new[] {  result.Error}
                   }
               };



            //var problemDetails = new ProblemDetails
            //{
            //    Status  = statusCode,
            //    Title = title,
            //    Extensions = new Dictionary<string, object?>()
            //   {{
            //           "errors",
            //             new[] {  result.Error}
            //       }
            //   }
            //};
            return new ObjectResult(problemDetails);



        }
    }
}
