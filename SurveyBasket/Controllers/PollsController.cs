
using Mapster;
using SurveyBasket.Contracts.Requests;
using SurveyBasket.Contracts.Responses;

namespace SurveyBasket.Controllers


{
    [ApiController]
    [Route("api/[controller]")]

   public class PollsController(PollServices pollServices) : ControllerBase
    {
        private readonly PollServices _pollServices = pollServices;

        [HttpGet]
        public ActionResult GetAll()
        {
            var polls = _pollServices.GetAll();
            var response = polls.Adapt<IEnumerable<PollResponse>>();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var poll = _pollServices.Get(id);
            if(poll is null)
                return NotFound();
            var response = poll.Adapt<PollResponse>();
            return Ok(response);
        }

        [HttpPost("")]
        public ActionResult Add(CreatePollRequest Request)
        {
            var NewPoll = _pollServices.Add(Request.Adapt<Poll>());

            return CreatedAtAction(nameof(Get), new { id = NewPoll.Id }, NewPoll);
        }



        [HttpPut("")]
        public ActionResult Update(int id , CreatePollRequest Request)
        {
            var IsUpdated = _pollServices.Update(id, Request.Adapt<Poll>());
            if (!IsUpdated)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id) { 
        var IsDeleted = _pollServices.Delete(id);
            if(!IsDeleted)
                return NotFound();
            return NoContent();
        }
    }



}

