namespace Backend_App_Tareas_Hogar.Application.Users.Login
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public List<string> Roles { get; set; }
        public string Message { get; set; }

        // Factory: éxito
        public static LoginResponse Success(string token, List<string> roles) =>
            new LoginResponse
            {
                Token = token,
                Roles = roles,
                Message = "Inicio de sesión exitoso."
            };

        // Factory: credenciales inválidas
        public static LoginResponse InvalidCredentials() =>
            new LoginResponse
            {
                Token = "",
                Roles = new List<string>(),
                Message = "Credenciales inválidas."
            };
    }

}
