
using Backend_App_Tareas_Hogar.Infraestructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Application.Users.Login
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder 50 caracteres.")
                .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres.")
                .MustAsync(async (username, ct) =>
                    await dbContext.Users.AnyAsync(u => u.Username.ToUpper() == username.ToUpper(), ct)
                )
                .WithMessage("El nombre de usuario no existe");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
        }
    }
}
