using Backend_App_Tareas_Hogar.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend_App_Tareas_Hogar.Application.Users.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, LogoutResponse>
    {
        private readonly ApplicationDbContext _dbContext;

        public LogoutHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LogoutResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // Buscar el refresh token en la base de datos
            var token = await _dbContext.UserTokens 
                .FirstOrDefaultAsync(t => t.RefreshToken == request.RefreshToken, cancellationToken);

            if (token == null)
                return LogoutResponse.NotFound();

            token.IsRevoked = true;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return LogoutResponse.Done();
        }
    }
}
