using Backend_App_Tareas_Hogar.Application.Users.Login;
using Backend_App_Tareas_Hogar.Infraestructure.Interfaces;
using MediatR;

namespace Backend_App_Tareas_Hogar.Application.Users.RefreshToken
{
    public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IJwtService _jwtService;
        public RefreshTokenHandler(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task<RefreshTokenResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            var tokenNew = await _jwtService.RefreshTokenAsync(request.RefreshToken);
            
            if(tokenNew.Token == null)
                return RefreshTokenResponse.Invalid(tokenNew.Message);

            return RefreshTokenResponse.Success(tokenNew.Token, tokenNew.Refreshtoken);
        }
    }
}
