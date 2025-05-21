
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyBasket.Contracts.Authentication;
using SurveyBasket.Contracts.Poll;
using SurveyBasket.Presistence.Entites;
using SurveyBasket.Services.NewFolder;
using System.Threading;

namespace SurveyBasket.Controllers


{
    [ApiController]
    [Route("api/[controller]")]

     [Authorize]
    public class PollsController(IPollServices pollServices) : ControllerBase
    {
        private readonly IPollServices _pollServices = pollServices;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollServices.GetAllAsync(cancellationToken);
            return Ok(polls);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var poll = await _pollServices.GetAsync(id, cancellationToken);
            return poll.IsSuccess ? Ok(poll.Value) : poll.ToProblem(StatusCodes.Status400BadRequest);

        }

        [HttpPost("")]
        public async Task<IActionResult> Add(PollRequest Request, CancellationToken cancellationToken, [FromServices] IValidator<PollRequest> Validator)
        {   
            var NewPoll = await _pollServices.AddAsync(Request, cancellationToken);
            return CreatedAtAction(nameof(Get), new { ID = NewPoll.Value.id }, NewPoll);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PollRequest Request, CancellationToken cancellationToken)
        {
            var result = await _pollServices.UpdateAsync(id, Request, cancellationToken);
            
            return result.IsSuccess? NoContent() : result.ToProblem(StatusCodes.Status400BadRequest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var IsDeleted = await _pollServices.DeleteAsync(id, cancellationToken);
            return IsDeleted.IsSuccess? NoContent() : IsDeleted.ToProblem(StatusCodes.Status400BadRequest);
        }
        [AllowAnonymous]
        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
        {
            var IsUpdated = await _pollServices.TogglePublishAsync(id, cancellationToken);
            return IsUpdated.IsSuccess ? NoContent() : IsUpdated.ToProblem(StatusCodes.Status400BadRequest);
        }

    }



}


