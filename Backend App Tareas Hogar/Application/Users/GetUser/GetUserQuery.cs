using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Backend_App_Tareas_Hogar.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Application.Users.GetUser
{
    public class GetUserQuery : IRequest<GetUserResponse>
    {
        public Guid UserId { get; set; }
    }

    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator(ApplicationDbContext dbContext)
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("El ID de usuario es obligatorio.")
                .MustAsync(async (userId, ct) =>
                    await dbContext.Users.AnyAsync(u => u.Id == userId, ct)
                )
                .WithMessage(x => $"El usuario no existe con ID: {x.UserId}");
        }
    }
}
