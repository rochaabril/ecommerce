using EcommerceApi.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceApi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(Usuario usuario)
        {
            // Leer la clave secreta desde appsettings.json
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            // Crear los claims que incluirán el nombre de usuario y el rol
            var claims = new List<Claim>
            {
			    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
				new Claim(ClaimTypes.Name, usuario.Email), // O el identificador único que prefieras
                new Claim(ClaimTypes.Role, usuario.Role.Nombre) // Asegúrate de que el rol esté bien asignado
            };
			
			var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Define el tiempo de expiración del token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token); // Regresa el token como string
        }
    }
}

