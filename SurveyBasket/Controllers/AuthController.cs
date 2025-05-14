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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] AuthRequst requst , CancellationToken cancellationToken)
        {
            var AuthResult = await _authService.GetTokenAsync(requst.Email, requst.Password, cancellationToken);

            return AuthResult is null ? BadRequest("invalid EMAIL/PASSWORD") : Ok(AuthResult);
        }

      

    }
}
