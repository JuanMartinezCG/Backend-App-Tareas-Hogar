using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Backend_App_Tareas_Hogar.Infraestructure.Interfaces;
using Backend_App_Tareas_Hogar.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Application.Users.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtService _jwtService;
        private readonly PasswordHasher<User> _passwordHasher;

        public LoginHandler(ApplicationDbContext dbContext, IJwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Buscar usuario con roles
            var user = await _dbContext.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Username.ToUpper() == request.UserName.ToUpper(), cancellationToken);

            // Validación unificada (usuario inexistente o contraseña incorrecta)
            var resultHash = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password.Trim());
            if (user == null || resultHash == PasswordVerificationResult.Failed)
            {
                return LoginResponse.InvalidCredentials();
            }


            // Construir lista de roles
            var roles = user.UserRoles.Select(r => r.Role.Name).ToList();

            // Generar token
            var token = _jwtService.GenerateToken(
                user.Id,
                user.Username,
                roles
            );

            // Respuesta final
            return LoginResponse.Success(token, roles);
        }
    }
}
