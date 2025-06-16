using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Contracts.Account;

namespace SurveyBasket.Controllers
{
    [Route("/me")]
    [ApiController]
    [Authorize]
    public class AccountController(IAccountService accountService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;

        [HttpGet("")]
        public async Task<IActionResult> GetProfile()
        {

            var result = await _accountService.GetProfileAsync(User.GetUserId()!);

            return Ok(result.Value);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateProfile([FromForm] UpdateProfileRequest request)
        {
            var result = await _accountService.UpdateProfileAsync(User.GetUserId()!, request);

            return NoContent();
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var result = await _accountService.ChangePasswordAsync(User.GetUserId()!, request);
            return result.IsSuccess ? NoContent() : result.ToProblem();

        }
    }
}
