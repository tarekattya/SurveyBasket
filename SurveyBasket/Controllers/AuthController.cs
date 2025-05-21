using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Options;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequst requst , CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.GetTokenAsync(requst.Email, requst.Password, cancellationToken);

            return AuthResult.IsSuccess ? Ok(AuthResult.Value) : BadRequest(AuthResult.Error);
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest tokenRequest , CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.GetRefreshTokenAsync(tokenRequest.Token, tokenRequest.RefreshToken, cancellationToken);

            return AuthResult is null ? BadRequest("invalid Token") : Ok(AuthResult);
        }

        [HttpPut("Revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            var IsRevoked = await _authService.RevokeRefreshTokenAsync(tokenRequest.Token, tokenRequest.RefreshToken, cancellationToken);

            return IsRevoked ? Ok("Done") : BadRequest("Operation Failed");
        }

    }
}
