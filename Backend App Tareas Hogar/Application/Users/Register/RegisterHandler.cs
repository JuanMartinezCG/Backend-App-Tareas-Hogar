using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Backend_App_Tareas_Hogar.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Application.Users.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
    {
        private readonly ApplicationDbContext _DBcontext;
        private readonly PasswordHasher<User> _passwordHasher;
        public RegisterHandler(ApplicationDbContext DBcontext)
        {
            _DBcontext = DBcontext;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Crear nuevo usuario
            var user  = new User
            {
                Name =  request.Name.ToUpper(),
                LastName = request.LastName.ToUpper(),
                Username = request.UserName,
                Age = request.Age
            };

            // Hashear contraseña
            user.Password = _passwordHasher.HashPassword(user, request.Password.Trim());

            // Obtener roles desde la base de datos
            var roles = await _DBcontext.Roles
            .Where(r => request.RoleId.Contains(r.Id))
            .ToListAsync(cancellationToken);

            // Crear relaciones User-Roles
            user.UserRoles = roles.Select(r => new UserRole
            {
                RoleId = r.Id
            }).ToList();

            /*
            // Agregar usuario a la base de datos
            var permisos = await _DBcontext.Permissions
                .Where(p => request.PermissionId.Contains(p.Id))
                .ToListAsync(cancellationToken);

            // Crear lista UserPermission
            user.UserPermissions = permisos.Select(p => new UserPermission
            {
                PermissionId = p.Id
            }).ToList();*/

            _DBcontext.Users.Add(user);
            await _DBcontext.SaveChangesAsync(cancellationToken);

            return new RegisterResponse 
            {
                UserId = user.Id,
                Message = "Usuario registrado exitosamente"
            };
        }
    }
}
