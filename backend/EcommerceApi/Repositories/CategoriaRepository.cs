using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
	public class CategoriaRepository:ICategoriaRepository
	{
		private readonly tienda_mayoristaContext _context;

		public CategoriaRepository(tienda_mayoristaContext context)
		{
			_context = context;
		}

		public async Task AddAsync(CategoriaDTO categoria)
		{
			var category = categoria.ToEntity(); // Mapea DTO a entidad
			_context.Categorias.Add(category);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var categoria = await _context.Categorias.FindAsync(id);
			if (categoria != null)
			{
				_context.Categorias.Remove(categoria);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<CategoriaDTO>> GetAllAsync()
		{
			var categoria = await _context.Categorias.ToListAsync();
			return categoria.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
		}

		public async Task<CategoriaDTO> GetByIdAsync(int id)
		{
			var categoria = await _context.Categorias.FindAsync(id);
			return categoria?.ToDTO();  // Mapea entidad a DTO
		}

		public async Task UpdateAsync(CategoriaDTO categoria)
		{
			var category = categoria.ToEntity();  // Mapea DTO a entidad
			_context.Categorias.Update(category);
			await _context.SaveChangesAsync();
		}
	}
}
