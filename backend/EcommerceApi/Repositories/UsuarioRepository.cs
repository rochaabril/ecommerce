using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
	public class UsuarioRepository:IUsuarioRepository
	{
		private readonly tienda_mayoristaContext _context;

		public UsuarioRepository(tienda_mayoristaContext context)
		{
			_context = context;
		}

        public async Task<string> GetRoleNameByIdAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            return role?.Nombre ?? string.Empty;
        }

        public async Task<UsuarioDTO> AddAsync(UsuarioDTO persona)
        {
            // Convertir el DTO a la entidad del modelo si es necesario
            var usuario = new Usuario
            {
                Nombre = persona.Nombre,
                Email = persona.Email,
                Password = persona.Password,
                RoleId = persona.RoleId,
                Direccion = persona.Direccion,
                Whatsapp = persona.Whatsapp,
                // Otros campos...
            };

            // Agregar la entidad al contexto
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            // Convertir la entidad creada de nuevo al DTO
            return new UsuarioDTO
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Password = usuario.Password,
                RoleId = usuario.RoleId,
                Direccion=usuario.Direccion,
                Whatsapp=usuario.Whatsapp,
                // Otros campos...
            };
        }

        public async Task DeleteAsync(int id)
		{
			var usuario = await _context.Usuarios.FindAsync(id);
			if (usuario != null)
			{
				_context.Usuarios.Remove(usuario);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<UsuarioDTO>> GetAllAsync()
		{
			var usuario = await _context.Usuarios.ToListAsync();
			return usuario.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
		}

		public async Task<UsuarioDTO> GetByEmailAsync(string email)
		{
			var usuario= await _context.Usuarios.
			FirstOrDefaultAsync(u => u.Email == email);

			return usuario.ToDTO();

		}

		public async Task<UsuarioDTO> GetByIdAsync(int id)
		{
			var usuario = await _context.Usuarios.FindAsync(id);
			return usuario?.ToDTO();  // Mapea entidad a DTO
		}

        //public async Task UpdateAsync(UsuarioDTO usuarioDTO)
        //{
        //	var usuario = usuarioDTO.ToEntity();  // Mapea DTO a entidad
        //	_context.Usuarios.Update(usuario);
        //	await _context.SaveChangesAsync();
        //}
        public async Task UpdateAsync(UsuarioDTO usuarioDTO)
        {
            // Buscar la entidad existente en el contexto
            var usuarioExistente = await _context.Usuarios.FindAsync(usuarioDTO.Id);

            if (usuarioExistente == null)
                throw new KeyNotFoundException("Usuario no encontrado.");

            // Actualizar los campos necesarios
            usuarioExistente.Password = usuarioDTO.Password;
            usuarioExistente.RememberToken = usuarioDTO.RememberToken;

            // Guardar los cambios en el contexto
            _context.Usuarios.Update(usuarioExistente);
            await _context.SaveChangesAsync();
        }


        public async Task SaveResetTokenAsync(int userId, string token)
		{
			var user = await _context.Usuarios.FindAsync(userId);
			if (user != null)
			{
				user.RememberToken = token;
				await _context.SaveChangesAsync();
			}
		}

		public async Task<Usuario> GetByResetTokenAsync(string token)
		{
			return await _context.Usuarios.FirstOrDefaultAsync(u => u.RememberToken == token);
		}
	}
}
