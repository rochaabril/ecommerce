using EcommerceApi.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;


namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly IConfiguration _configuration;
		private readonly CarritoService _carritoService;

		public UsuarioController(UsuarioService usuarioService, IConfiguration configuration, CarritoService carritoService)
		{
			_usuarioService = usuarioService;
			_configuration = configuration;
			_carritoService = carritoService;
		}


		//[HttpPost("login")]
		//public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
		//{
		//	// Validar usuario y obtener el objeto de usuario
		//	var user = await _usuarioService.ValidateUser(loginDTO.Email, loginDTO.Password);

		//	if (user == null)
		//	{
		//		return Unauthorized(); // Devuelve 401 si no es válido
		//	}

		//	// Generar el token
		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

		//	var tokenDescriptor = new SecurityTokenDescriptor
		//	{
		//		Subject = new ClaimsIdentity(new Claim[]
		//		{
		//	new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
		//	new Claim(ClaimTypes.Email, user.Email)
		//		}),
		//		Expires = DateTime.UtcNow.AddDays(7), // Considera ajustar este valor
		//		SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		//	};

		//	var token = tokenHandler.CreateToken(tokenDescriptor);

		//	return Ok(new { Token = tokenHandler.WriteToken(token) }); // Devuelve el token
		//}


		[HttpGet]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> GetAll()
        {
            var personas = await _usuarioService.GetAllAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
		[Authorize(Roles = "Admin,Cliente")]

		public async Task<IActionResult> GetById(int id)
        {
            var persona = await _usuarioService.GetByIdAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return Ok(persona);
        }

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] UsuarioDTO usuarioDTO)
		{
			// Hash de la contraseña
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuarioDTO.Password);
			usuarioDTO.Password = hashedPassword; // Reemplaza la contraseña con el hash

			// Crear el usuario
			var user = await _usuarioService.AddAsync(usuarioDTO);

			// Crear el carrito asociado
		
			return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
		}
		[HttpPut("{id}")]
		[Authorize(Roles = "Admin,Cliente")]

		public async Task<IActionResult> Update(int id, [FromBody] UsuarioDTO personaDTO)
        {
            if (id != personaDTO.Id)
            {
                return BadRequest();
            }

            await _usuarioService.UpdateAsync(personaDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Delete(int id)
        {
            await _usuarioService.DeleteAsync(id);
            return NoContent();
        }

		[HttpPost("request-reset-password")]
		public async Task<IActionResult> RequestResetPassword([FromBody] string email)
		{
			//var result = await _usuarioService.RequestPasswordReset(email);
			//return Ok(result);
			try
			{
				var result = await _usuarioService.RequestPasswordReset(email);
				return Ok(result);
			}
			catch (Exception ex)
			{
				return StatusCode(500, ex.Message); // Retornar un mensaje de error específico
			}
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
		{
			try
			{
				await _usuarioService.ResetPassword(model.Token, model.NewPassword);
				return Ok(new { success = true, message = "Contraseña restablecida exitosamente." });
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}
	}
}
