using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;

namespace EcommerceApi.Services
{
	public class UsuarioService
	{
        private readonly IUsuarioRepository _usuarioRepository;
		private readonly EmailService _emailService;


		public UsuarioService(IUsuarioRepository usuarioRepository, EmailService emailService)
		{
			_usuarioRepository = usuarioRepository;
			_emailService = emailService;
		}

		public Task<IEnumerable<UsuarioDTO>> GetAllAsync() => _usuarioRepository.GetAllAsync();
        public Task<UsuarioDTO> GetByIdAsync(int id) => _usuarioRepository.GetByIdAsync(id);
        public async Task<UsuarioDTO> AddAsync(UsuarioDTO persona)
        {
            return await _usuarioRepository.AddAsync(persona);
        }
        public Task UpdateAsync(UsuarioDTO persona) => _usuarioRepository.UpdateAsync(persona);
        public Task DeleteAsync(int id) => _usuarioRepository.DeleteAsync(id);

		public async Task ResetPassword(string token, string newPassword)
		{
			var user = await _usuarioRepository.GetByResetTokenAsync(token);
			if (user == null)
			{
				throw new Exception("Token inválido o expirado.");
			}

			user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); // Hashear la nueva contraseña
			user.RememberToken = null; // Limpiar el token
			await _usuarioRepository.UpdateAsync(user.ToDTO());
		}

		public async Task<string> RequestPasswordReset(string email)
		{
			var user = await _usuarioRepository.GetByEmailAsync(email);
			if (user == null)
			{
				throw new Exception("Usuario no encontrado.");
			}

			var token = Guid.NewGuid().ToString(); // Generar un token único
			await _usuarioRepository.SaveResetTokenAsync(user.Id, token); // Guardar el token en la base de datos

			// Aquí puedes implementar la lógica para enviar un correo
			var resetLink = $"http://localhost:4200/reset/reset-password?token={token}";
			await _emailService.SendResetPasswordEmailAsync(email, resetLink);

			return "Se ha enviado un enlace para restablecer la contraseña a tu correo.";
		}

		public async Task<UsuarioDTO> ValidateUser(string email, string password)
		{
			var user = await _usuarioRepository.GetByEmailAsync(email);
			if (user == null)
			{
				return null; // Usuario no encontrado
			}

			// Verifica la contraseña
			if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
			{
				return null; // Contraseña incorrecta
			}

            // Cargar el nombre del rol usando RoleId
            var roleName = await _usuarioRepository.GetRoleNameByIdAsync(user.RoleId);


            // Configura el DTO incluyendo el nombre del rol
            return new UsuarioDTO
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Email = user.Email,
                RoleId = user.RoleId,
                RoleName = roleName,
                Direccion = user.Direccion,
                Whatsapp = user.Whatsapp,
                EmailVerificado = user.EmailVerificado,
                RememberToken = user.RememberToken
            };

		}

	}
}
