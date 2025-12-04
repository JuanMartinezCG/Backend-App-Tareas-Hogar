using Backend_App_Tareas_Hogar.Infraestructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Application.Users.Register
{
    public class RegisterCommand : IRequest<RegisterResponse>
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        public List<int> RoleId { get; set; }
        //public ICollection<int> PermissionId { get; set; }
    }
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator(ApplicationDbContext dbContext)
        {
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
                .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder 50 caracteres.")
                .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres.")
                .MustAsync(async (username, ct) => !await dbContext.Users.AnyAsync(u => u.Username == username, ct))
                .WithMessage("El NickName ya existe");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.Age)
                .GreaterThan(0).WithMessage("La edad debe ser un número positivo.");


            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("Debe especificar al menos un rol.")
                .MustAsync(async (model, roles, context, ct) =>
                {
                    // Obtener los roles que sí existen
                    var rolesExistentes = await dbContext.Roles
                        .Where(r => roles.Contains(r.Id))
                        .Select(r => r.Id)
                        .ToListAsync(ct);

                    // Calcular los que NO existen
                    var rolesNoExisten = roles
                        .Distinct()
                        .Where(id => !rolesExistentes.Contains(id))
                        .ToList();

                    // Si hay roles inexistentes, los insertamos en el mensaje
                    if (rolesNoExisten.Count > 0)
                    {
                        context.MessageFormatter.AppendArgument( // Agregar argumento personalizado
                            "RolesInvalidos",
                            string.Join(", ", rolesNoExisten)
                        );

                        return false;
                    }

                    return true;
                })
                .WithMessage("Los siguientes roles no existen: {RolesInvalidos}");

            /*RuleFor(x => x.PermissionId)
                .CustomAsync(async (ids, context, ct) =>
                {
                    if (ids == null || !ids.Any())
                    {
                        context.AddFailure("La lista de permisos no puede estar vacía.");
                        return;
                    }

                    var duplicados = ids.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
                    if (duplicados.Any())
                    {
                        context.AddFailure($"Los siguientes permisos están duplicados: {string.Join(", ", duplicados)}");
                        return;
                    }

                    var permisosExistentes = await dbContext.Permissions
                        .Where(p => ids.Contains(p.Id))
                        .Select(p => p.Id)
                        .ToListAsync(ct);

                    var NoExiste = ids.Except(permisosExistentes).ToList();
                    if (NoExiste.Any())
                    {
                        context.AddFailure($"Los siguientes permisos no existen: {string.Join(", ", NoExiste)}");
                    }
                });*/
        }
    }
}
