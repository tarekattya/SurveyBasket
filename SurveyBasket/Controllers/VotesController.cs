using SurveyBasket.Contracts.Vote;

namespace SurveyBasket.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
[Authorize]
public class VotesController(IQuestionService questionService, IVoteServices voteService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteServices _voteService = voteService;

    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _questionService.GetAvaliableAll(pollId, userId!, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var result = await _voteService.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }
}