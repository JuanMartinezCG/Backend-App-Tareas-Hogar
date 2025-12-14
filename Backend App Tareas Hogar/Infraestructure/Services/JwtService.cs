using Backend_App_Tareas_Hogar.Application.Users.Login;
using Backend_App_Tareas_Hogar.Application.Users.Logout;
using Backend_App_Tareas_Hogar.Application.Users.RefreshToken;
using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Backend_App_Tareas_Hogar.Infraestructure.Interfaces;
using Backend_App_Tareas_Hogar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend_App_Tareas_Hogar.Infraestructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public JwtService(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateToken(User user, IEnumerable<string> roles)
        {
            // Claims 
            var claims = new List<Claim>
                {
                    new Claim("uid", user.Id.ToString()),
                    new Claim("name", user.Name.ToString()), // Nombre del usuario
                    new Claim("lastName", user.LastName.ToString()), // Apellido del usuario
                    new Claim("username", user.Username.ToString()), // Nombre de usuario 
                    new Claim("age", user.Age.ToString()) // Edad del usuario
                };

            // Agregar roles como claims
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // Validar que la clave secreta no sea nula o vacía
            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("La clave secreta del token JWT no está configurada.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Lee desde appsettings.json

            // Clave generada para firmar el token con algoritmo HMAC-SHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Construir el token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), // Expira en 2h
                signingCredentials: creds
            );

            // Convertirlo a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }   

        public string GenerateRefreshToken()
        {
            // Generamos un token de actualización aleatorio
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber); // Convertimos a Base64 para que sea legible y seguro
            }
        }

        public async Task<LoginResponse>
            GenerateAndSaveTokensAsync(User user, IEnumerable<string> roles)
        {
            var accessToken = GenerateToken(user, roles);
            var refreshToken = GenerateRefreshToken();

            var entity = new UserToken
            {
                UserId = user.Id, // ID del usuario
                RefreshToken = refreshToken, // Token generado
                RefreshTokenExpiration = DateTime.Now.AddHours(12), // Expira en 12 horas
                CreatedAt = DateTime.Now,
                IsRevoked = false // Nuevo token, no revocado
            };

            _context.UserTokens.Add(entity);
            await _context.SaveChangesAsync();

            return LoginResponse.Success(
                accessToken,
                refreshToken
            );
        }

        // ======================================================
        // REFRESH TOKEN LOGIC
        // ======================================================
        public async Task<RefreshTokenResponse> RefreshTokenAsync(string refreshToken)
        {
            var storedToken = await _context.UserTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.RefreshToken == refreshToken &&
                    !x.IsRevoked); 

            if (storedToken == null)
                return RefreshTokenResponse.Invalid("Refresh token no encontrado o revocado.");

            if (storedToken.RefreshTokenExpiration < DateTime.Now)
                return RefreshTokenResponse.Invalid("El refresh token expiró.");

            var user = storedToken.User;

            // Revocar token anterior
            storedToken.IsRevoked = true;

            // Obtener roles del usuario
            var roles = await _context.UserRoles
                .Where(x => x.UserId == user.Id)
                .Select(x => x.Role.Name)
                .ToListAsync();

            // Crear nuevos tokens
            var newAccess = GenerateToken(user, roles);
            var newRefresh = GenerateRefreshToken();

            var newTokenEntity = new UserToken
            {
                UserId = user.Id,
                RefreshToken = newRefresh,
                RefreshTokenExpiration = DateTime.Now.AddHours(12),
                CreatedAt = DateTime.Now,
                IsRevoked = false
            };

            _context.UserTokens.Add(newTokenEntity);

            await _context.SaveChangesAsync();

            return RefreshTokenResponse.Success(
                newAccess,
                newRefresh
            );
        }
    }
}
