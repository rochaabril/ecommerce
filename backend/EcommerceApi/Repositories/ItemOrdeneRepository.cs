using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
	public class ItemOrdeneRepository:IItemOrdeneRepository
	{
		private readonly tienda_mayoristaContext _context;

		public ItemOrdeneRepository(tienda_mayoristaContext context)
		{
			_context = context;
		}

		public async Task AddAsync(ItemOrdeneDTO itemOrdene)
		{
			var itemOrde = itemOrdene.ToEntity(); // Mapea DTO a entidad
			_context.ItemOrdenes.Add(itemOrde);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var itemOrdene = await _context.ItemOrdenes.FindAsync(id);
			if (itemOrdene != null)
			{
				_context.ItemOrdenes.Remove(itemOrdene);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<ItemOrdeneDTO>> GetAllAsync()
		{
			var itemOrdenes = await _context.ItemOrdenes.ToListAsync();
			return itemOrdenes.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
		}

		public async Task<ItemOrdeneDTO> GetByIdAsync(int id)
		{
			var itemOrdene = await _context.ItemOrdenes.FindAsync(id);
			return itemOrdene?.ToDTO();  // Mapea entidad a DTO
		}

		public async Task UpdateAsync(ItemOrdeneDTO itemOrdene)
		{
			var itemOrd = itemOrdene.ToEntity();  // Mapea DTO a entidad
			_context.ItemOrdenes.Update(itemOrd);
			await _context.SaveChangesAsync();
		}
	}
}
