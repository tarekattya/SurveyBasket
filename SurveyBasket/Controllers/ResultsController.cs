using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Services;

namespace SurveyBasket.Controllers
{
    [Route("api/Polls/{PollId}/[controller]")]
    [ApiController]
    [HasPermission(Permissions.Results_Read)]
    public class ResultsController(IResultService resultService) : ControllerBase
    {
        private readonly IResultService _resultService = resultService;

        [HttpGet("row-data")]
        public async Task<IActionResult> PollVotes([FromRoute] int PollId, CancellationToken cancellationToken = default)
        {
            var result = await _resultService.GetResult(PollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [HttpGet("votes-per-day")]
        public async Task<IActionResult> VotesPerDay([FromRoute] int PollId, CancellationToken cancellationToken = default)
        {
            var result = await _resultService.GetVotesPerDayAsync(PollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }

        [HttpGet("votes-per-questions")]
        public async Task<IActionResult> VotesPerQuestions([FromRoute] int PollId, CancellationToken cancellationToken = default)
        {
            var result = await _resultService.GetVotesPerQuestionAsync(PollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

        }


    }
}
