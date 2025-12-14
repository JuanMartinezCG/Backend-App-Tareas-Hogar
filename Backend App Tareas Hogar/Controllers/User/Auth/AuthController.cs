using Backend_App_Tareas_Hogar.Application.Users.Login;
using Backend_App_Tareas_Hogar.Application.Users.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_App_Tareas_Hogar.Controllers.User.Auth
{
    [ApiController]
    [Route("api/[controller]")] // La ruta será api/auth
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")] // URL: api/auth/login
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

       [HttpPost("refresh")]
       [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
       [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }
}
