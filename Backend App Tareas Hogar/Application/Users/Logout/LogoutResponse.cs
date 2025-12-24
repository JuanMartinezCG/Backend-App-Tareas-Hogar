namespace Backend_App_Tareas_Hogar.Application.Users.Logout
{
    public class LogoutResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public static LogoutResponse Done() =>
            new() { Success = true, Message = "Sesión cerrada correctamente." };

        public static LogoutResponse NotFound() =>
            new() { Success = false, Message = "El token no existe o ya fue revocado." };
    }

}
