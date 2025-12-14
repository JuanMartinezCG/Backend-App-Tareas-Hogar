using Backend_App_Tareas_Hogar.Application.Users.Login;
using Backend_App_Tareas_Hogar.Infraestructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Application.Users.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("El refresh token es obligatorio.");
        }
    }
}
