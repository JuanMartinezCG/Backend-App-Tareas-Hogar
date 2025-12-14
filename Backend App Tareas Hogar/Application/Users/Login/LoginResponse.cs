namespace Backend_App_Tareas_Hogar.Application.Users.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string Refreshtoken { get; set; }
        public string Message { get; set; }

        // Factory: éxito
        public static LoginResponse Success(string token, string refreshtoken) =>
            new LoginResponse
            {
                Token = token,
                Refreshtoken = refreshtoken,
                Message = "Inicio de sesión exitoso."
            };

        // Factory: credenciales inválidas
        public static LoginResponse InvalidCredentials() =>
            new LoginResponse
            {
                Token = "",
                Refreshtoken = "",
                Message = "Credenciales inválidas."
            };

        public static LoginResponse Invalid(string message) =>
        new()
        {
            Message = message
        };
    }

}
