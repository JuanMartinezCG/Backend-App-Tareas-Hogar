using Backend_App_Tareas_Hogar.Infraestructure.Data;
using MediatR;

namespace Backend_App_Tareas_Hogar.Application.User.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly ApplicationDbContext _BDcontext;

        public LoginHandler(ApplicationDbContext BDcontext)
        {
            _BDcontext = BDcontext;
        }


    }
}
