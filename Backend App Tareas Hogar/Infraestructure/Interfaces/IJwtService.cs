using Backend_App_Tareas_Hogar.Application.Users.Login;
using Backend_App_Tareas_Hogar.Application.Users.Logout;
using Backend_App_Tareas_Hogar.Application.Users.RefreshToken;
using Backend_App_Tareas_Hogar.Models;

namespace Backend_App_Tareas_Hogar.Infraestructure.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user, IEnumerable<string> roles);
        string GenerateRefreshToken();

        Task<LoginResponse> GenerateAndSaveTokensAsync(User user, IEnumerable<string> roles);
        Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken);
    }

}
