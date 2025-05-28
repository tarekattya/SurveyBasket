﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SurveyBasket.Controllers
{
    [Route("api/Polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController(IQuestionService questionService) : ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;



        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAllAsync(pollId, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return result.ToProblem();


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id, [FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAsync(pollId, id, cancellationToken);
            if (result.IsSuccess)
                return Ok(result.Value);
            return result.ToProblem();
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionService.AddAsync(pollId, request, cancellationToken);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);


            return result.ToProblem();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
        {
            var result = await _questionService.UpdateAsync(pollId,id, request, cancellationToken);

            if (result.IsSuccess)
                return NoContent();


            return result.ToProblem();

        }

        [HttpPut("{id}/ToggleStatus")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int id, [FromRoute] int pollId, CancellationToken cancellationToken)
        {
            var result = await _questionService.ToggleStatus(pollId, id, cancellationToken);
            if (result.IsSuccess)
                return NoContent();
            return result.ToProblem();

        }
    }
}
