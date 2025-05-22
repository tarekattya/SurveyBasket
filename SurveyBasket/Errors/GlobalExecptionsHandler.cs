using Microsoft.AspNetCore.Diagnostics;

namespace SurveyBasket.Errors
{
    public class GlobalExecptionsHandler(ILogger<GlobalExecptionsHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<GlobalExecptionsHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An error occurred while processing the request {message}  ", exception.Message);
            var problemDetails = new ProblemDetails
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
           await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);


            return true;
        }
    }
}
