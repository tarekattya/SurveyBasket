using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Authentication.Filters;
using SurveyBasket.Contracts.Roles;

namespace SurveyBasket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleServices roleServices) : ControllerBase
    {
        private readonly IRoleServices _roleServices = roleServices;

        [HttpGet("")]
        [HasPermission(Permissions.Roles_Read)]
        public async Task<IActionResult> GetAll([FromQuery] bool IncludeDisable , CancellationToken cancellationToken = default)
        {
            var roles = await _roleServices.GetAllAsync(IncludeDisable);

            return Ok(roles);
        }
        [HttpGet("{id}")]
        [HasPermission(Permissions.Roles_Read)]
        public async Task<IActionResult> Get([FromRoute] string id)
        {
            var result = await _roleServices.GetAsync(id);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("")]
        [HasPermission(Permissions.Roles_Add)]
        public async Task<IActionResult> Add([FromBody] RoleRequest roleRequest)
        {
            var result = await _roleServices.AddAsync(roleRequest);

            return result.IsSuccess ? CreatedAtAction(nameof(Get) , new { result.Value.id} ,  result.Value) : result.ToProblem();
        }
        [HttpPut("{id}")]
        [HasPermission(Permissions.Roles_Update)]
        public async Task<IActionResult> Add([FromRoute] string id ,[FromBody] RoleRequest roleRequest)
        {
            var result = await _roleServices.UpdateAsync(id,roleRequest);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("{id}/ToggleStatus")]
        [HasPermission(Permissions.Roles_Update)]
        public async Task<IActionResult> TogglePublish([FromRoute] string id)
        {
            var result = await _roleServices.TooglePublish(id);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
