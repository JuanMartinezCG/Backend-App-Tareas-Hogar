using Backend_App_Tareas_Hogar.Infraestructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend_App_Tareas_Hogar.Infraestructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(Guid userId, string nickName, IEnumerable<string> roles)
        {
            // Key secreta para firmar el token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _config["Jwt:Key"]
            ));

            // Credenciales para el algoritmo de firma
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims 
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Nickname, nickName),
                    new Claim("uid", userId.ToString())
                };

            // Agregar roles como claims
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Construir el token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3), // Expira en 3h
                signingCredentials: creds
            );

            // Convertirlo a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
