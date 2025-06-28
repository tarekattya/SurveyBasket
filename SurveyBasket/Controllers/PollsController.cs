


using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Common;

namespace SurveyBasket.Controllers


{
    [ApiController]
    [Route("api/[controller]")]

    public class PollsController(IPollServices pollServices) : ControllerBase
    {
        private readonly IPollServices _pollServices = pollServices;

        [HttpGet("")]
        [HasPermission(Permissions.Polls_Read)]
        public async Task<IActionResult> GetAll(FilterRequest request, CancellationToken cancellationToken)
        => Ok(await _pollServices.GetAllAsync(request ,cancellationToken));

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent(FilterRequest request, CancellationToken cancellationToken)
       => Ok(await _pollServices.GetCurrentAsync(request,cancellationToken));

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var poll = await _pollServices.GetAsync(id, cancellationToken);
            return poll.IsSuccess ? Ok(poll.Value) : poll.ToProblem();

        }

        [HttpPost("")]
        public async Task<IActionResult> Add(PollRequest Request, CancellationToken cancellationToken, [FromServices] IValidator<PollRequest> Validator)
        {   
            var result = await _pollServices.AddAsync(Request, cancellationToken);

            if (result.IsSuccess)
              return  CreatedAtAction(nameof(Get), new { ID = result.Value.id }, result.Value);


            return result.Error.Equals(PollErrors.DublicateTitles) ? 
                 result.ToProblem() 
                 :
                 result.ToProblem();
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PollRequest Request, CancellationToken cancellationToken)
        {
            var result = await _pollServices.UpdateAsync(id, Request, cancellationToken);

            if (result.IsSuccess)
                return NoContent();


            return result.Error.Equals(PollErrors.DublicateTitles) ?
                 result.ToProblem()
                 :
                 result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var IsDeleted = await _pollServices.DeleteAsync(id, cancellationToken);
            return IsDeleted.IsSuccess? NoContent() : IsDeleted.ToProblem();
        }
        [AllowAnonymous]
        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
        {
            var IsUpdated = await _pollServices.TogglePublishAsync(id, cancellationToken);
            return IsUpdated.IsSuccess ? NoContent() : IsUpdated.ToProblem();
        }

    }



}


