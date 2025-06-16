using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SurveyBasket.Contracts.Account;
using SurveyBasket.Contracts.Authentication;
using SurveyBasket.Options;
using System.Reflection;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync([FromBody] AuthRequst requst, CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.GetTokenAsync(requst.Email, requst.Password, cancellationToken);

            return AuthResult.IsSuccess ? Ok(AuthResult.Value!) : AuthResult.ToProblem();
        }
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest tokenRequest , CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.GetRefreshTokenAsync(tokenRequest.Token, tokenRequest.RefreshToken, cancellationToken);

            return AuthResult.IsSuccess ? Ok(AuthResult) : AuthResult.ToProblem();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.RegisterAsync(request, cancellationToken);

            return AuthResult.IsSuccess ? Ok() : AuthResult.ToProblem();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.ConfirmEmailAsync(request, cancellationToken);

            return AuthResult.IsSuccess ? Ok() : AuthResult.ToProblem();
        }
        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmailAsync([FromBody] ResendConfirmationCodeRequest request, CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.ResendConfirmEmailAsync(request, cancellationToken);

            return AuthResult.IsSuccess ? Ok() : AuthResult.ToProblem();
        }

        [HttpPost("resend-reset-code")]
        public async Task<IActionResult> ResetCodeAsync([FromBody] ForgetPasswordRequest request)
        {
            var AuthResult = await _authService.ResendResetCodeAsync(request.Email);

            return AuthResult.IsSuccess ? Ok() : AuthResult.ToProblem();
        }
        [HttpPost("reset-Password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var AuthResult = await _authService.ResetPasswordAsync(request);

            return AuthResult.IsSuccess ? Ok() : AuthResult.ToProblem();
        }

        [HttpPut("Revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshAsync([FromBody] RefreshTokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            var IsRevoked = await _authService.RevokeRefreshTokenAsync(tokenRequest.Token, tokenRequest.RefreshToken, cancellationToken);

            return IsRevoked.IsSuccess ? Ok("Done") : IsRevoked.ToProblem(); ;
        }

    }
}
