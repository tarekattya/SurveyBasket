
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SurveyBasket.Contracts.Poll;
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

        [HttpGet("")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var polls = await _pollServices.GetAllAsync(cancellationToken);
            var response = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken)
        {
            var poll = await _pollServices.GetAsync(id, cancellationToken);
            if (poll is null)
                return NotFound();
            var response = poll.Adapt<PollResponse>();
            return Ok(response);
        }

        [HttpPost("")]
        public async Task<IActionResult> Add(CreatePollRequest Request,  CancellationToken cancellationToken, [FromServices] IValidator<CreatePollRequest> Validator )
        {
            var NewPoll = await _pollServices.AddAsync(Request.Adapt<Poll>(),  cancellationToken);
            return CreatedAtAction(nameof(Get), new { id = NewPoll.Id }, NewPoll);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CreatePollRequest Request , CancellationToken cancellationToken)
        {
            var IsUpdated =await _pollServices.UpdateAsync(id, Request.Adapt<Poll>(),cancellationToken);
            if (!IsUpdated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken)
        {
            var IsDeleted =await _pollServices.DeleteAsync(id,cancellationToken);
            if (!IsDeleted)
                return NotFound();
            return NoContent();
        }
        [HttpPut("{id}/TogglePublish")]
        public async Task<IActionResult> TogglePublish(int id, CreatePollRequest Request, CancellationToken cancellationToken)
        {
            var IsUpdated = await _pollServices.TogglePublishAsync(id,cancellationToken);
            if (!IsUpdated)
                return NotFound();
            return NoContent();
        }

    }



}


