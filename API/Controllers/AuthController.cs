using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Features.Auth.Login;
using Microsoft.AspNetCore.Identity.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    [Tags("Аутентификация")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return result.ToActionResult();
        }
    }
}