using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Common;
using SurveyBasket.Contracts.User;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserServices userServices) : ControllerBase
    {
        private readonly IUserServices _userServices = userServices;

        [HttpGet("")]
        [HasPermission(Permissions.Users_Read)]
        public async Task<IActionResult> GetAll([FromQuery] FilterRequest filter ,  CancellationToken cancellationToken = default)
        {
           var response = await _userServices.Getallasync(filter,cancellationToken);
            return Ok(response);
        }

        [HttpGet("{id}")]
        [HasPermission(Permissions.Users_Read)]
        public async Task<IActionResult> Get([FromRoute]string id)
        {   
            var response = await _userServices.getAsync(id);
            return Ok(response.Value);
        }


        [HttpPost("")]
        [HasPermission(Permissions.Users_Add)]
        public async Task<IActionResult> Add([FromBody] CreateUserRequest request)
        {
            var response = await _userServices.CreateAsync(request);

            return response.IsSuccess ? CreatedAtAction(nameof(Get), new { id = response.Value.id }, response.Value) : response.ToProblem();
        }

        [HttpPut("{id}")]
        [HasPermission(Permissions.Users_Update)]
        public async Task<IActionResult> Update([FromRoute] string id , [FromBody] UpdateUserRequest request)
        {
            var response = await _userServices.UpdateAsync(id , request);

            return response.IsSuccess ? NoContent() : response.ToProblem();

        }

        [HttpPut("{id}/toggle")]
        [HasPermission(Permissions.Users_Update)]
        public async Task<IActionResult> Toggle([FromRoute] string id)
        {
            var response = await _userServices.TooglePublish(id);

            return response.IsSuccess ? NoContent() : response.ToProblem();
        }
        [HttpPut("{id}/Unlocked")]
        [HasPermission(Permissions.Users_Update)]
        public async Task<IActionResult> Unlock([FromRoute] string id)
        {
            var response = await _userServices.UnLocked(id);

            return response.IsSuccess ? NoContent() : response.ToProblem();
        }

    }
}
