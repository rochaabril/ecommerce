using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly tienda_mayoristaContext _context;

        public RoleRepository(tienda_mayoristaContext context)
        {
            _context = context;
        }


        public async Task AddAsync(RoleDTO roleDTO)
        {
            var role = roleDTO.ToEntity();  // Mapea DTO a entidad
			_context.Roles.Add(role);
			await _context.SaveChangesAsync();
		}

        public async Task DeleteAsync(int id)
        {
			var role = await _context.Roles.FindAsync(id);
			if (role != null)
			{
				_context.Roles.Remove(role);
				await _context.SaveChangesAsync();
			}
		}

        public async Task<IEnumerable<RoleDTO>> GetAllAsync()
        {
			var roles = await _context.Roles.ToListAsync();
			return roles.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
		}

        public async Task<RoleDTO> GetByIdAsync(int id)
        {
			var role = await _context.Roles.FindAsync(id);
			return role?.ToDTO();  // Mapea entidad a DTO
		}

        public async Task UpdateAsync(RoleDTO roleDTO)
        {
			var role = roleDTO.ToEntity();  // Mapea DTO a entidad
			_context.Roles.Update(role);
			await _context.SaveChangesAsync();
		}
    }
}
