using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
	public class OrdeneRepository:IOrdeneRepository
	{
		private readonly tienda_mayoristaContext _context;

		public OrdeneRepository(tienda_mayoristaContext context)
		{
			_context = context;
		}

		public async Task AddAsync(OrdeneDTO ordeneDTO)
		{
			var orden = ordeneDTO.ToEntity();  // Mapea DTO a entidad
			_context.Ordenes.Add(orden);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var orden = await _context.Ordenes.FindAsync(id);
			if (orden != null)
			{
				_context.Ordenes.Remove(orden);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<OrdeneDTO>> GetAllAsync()
		{
			var ordenes = await _context.Ordenes.ToListAsync();
			return ordenes.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
		}

		public async Task<OrdeneDTO> GetByIdAsync(int id)
		{
			var orden = await _context.Ordenes.FindAsync(id);
			return orden?.ToDTO();  // Mapea entidad a DTO
		}

		public async Task UpdateAsync(OrdeneDTO ordeneDTO)
		{
			var orden = ordeneDTO.ToEntity();  // Mapea DTO a entidad
			_context.Ordenes.Update(orden);
			await _context.SaveChangesAsync();
		}
	}
}
