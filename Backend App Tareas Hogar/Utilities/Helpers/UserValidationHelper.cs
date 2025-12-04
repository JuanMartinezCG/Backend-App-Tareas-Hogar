using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend_App_Tareas_Hogar.Utilities.Helpers
{
    public static class UserValidationHelper
    {
        /*public static async Task<bool> ValidarRol(
            ApplicationDbContext dbContext,
            IUserContextService userContext,
            string rolEsperado,
            CancellationToken cancellationToken)
        {
            var userId = userContext.GetUserId();

            var user = await dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            return user is not null
                   && user.Rol.Name.ToLower() == rolEsperado.ToLower();
        }

        public static async Task<bool> ValidarRoles(
            ApplicationDbContext dbContext,
            IUserContextService userContext,
            string rolEsperado,
            string rolEsperado2,
            CancellationToken cancellationToken)
        {

            var user = await dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            return user is not null
                   && (user.Rol.Name.ToLower() == rolEsperado.ToLower() || user.Rol.Name.ToLower() == rolEsperado2.ToLower());
        }*/
    }
}
