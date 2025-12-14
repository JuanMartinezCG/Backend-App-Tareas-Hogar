using Backend_App_Tareas_Hogar.Application.Users.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_App_Tareas_Hogar.Controllers.User
{
    [ApiController] // Indica que este controlador maneja solicitudes HTTP y valida automáticamente el modelo
    [Route("api/[controller]")] // La ruta será api/auth
    //[Authorize]
    public class UserController : ControllerBase
    {
        
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[Authorize] 
        [HttpPost("register")] // URL: api/user/register
        [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(Register), result.UserId, result);
        }
    }
}
