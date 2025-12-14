using MediatR;

namespace Backend_App_Tareas_Hogar.Application.Users.Logout
{
    public class LogoutCommand : IRequest<LogoutResponse>
    {
        public string RefreshToken { get; set; }
    }

}
