using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Backend_App_Tareas_Hogar.Utilities.Dtos.User;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Application.Users.GetUser
{
    public class GetUserHandler : IRequestHandler<GetUserQuery, GetUserResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        public GetUserHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetUserResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserPermissions)
                .FirstAsync(u => u.Id == request.UserId, cancellationToken);

            return new GetUserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                LastName = user.LastName,
                Age = user.Age,
                DateRegister = user.CreatedAt,
                DataUpdate = user.UpdatedAt,
                Roles = user.UserRoles
                    .Select(r => new RoleDto
                    {
                        Id = r.Id, 
                        Name = r.Role.Name
                    })
                    .ToList(),

                Permissions = user.UserPermissions
                    .Select(p => new PermissionDto
                    {
                        Id = p.Id, 
                        Name = p.Permission.Name 
                    })
                    .ToList()
            };
        }
    }
}
