using Backend_App_Tareas_Hogar.Application.Users.Login;

namespace Backend_App_Tareas_Hogar.Application.Users.RefreshToken
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; }
        public string Refreshtoken { get; set; }
        public string Message { get; set; }
    
        public static RefreshTokenResponse Success(string token, string refreshtoken) =>
            new RefreshTokenResponse
            {
                Token = token,
                Refreshtoken = refreshtoken,
                Message = "Nuevo RefreshToken y Token Creado Exitosamente"
            };

        public static RefreshTokenResponse Invalid(string message) =>
            new RefreshTokenResponse
            {
                Message = message
            };
    }
}
