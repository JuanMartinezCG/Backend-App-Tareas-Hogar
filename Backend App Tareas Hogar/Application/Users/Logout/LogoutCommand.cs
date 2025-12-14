using Backend_App_Tareas_Hogar.Application.Users.RefreshToken;
using Backend_App_Tareas_Hogar.Infraestructure.Data;
using FluentValidation;
using MediatR;

namespace Backend_App_Tareas_Hogar.Application.Users.Logout
{
    public class LogoutCommand : IRequest<LogoutResponse>
    {
        public string RefreshToken { get; set; }
    }

    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("El refresh token es obligatorio.");
        }
    }

}
