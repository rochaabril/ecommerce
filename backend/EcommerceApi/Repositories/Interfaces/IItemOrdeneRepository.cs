using EcommerceApi.Dtos;

namespace EcommerceApi.Repositories.Interfaces
{
	public interface IItemOrdeneRepository
	{
		Task<IEnumerable<ItemOrdeneDTO>> GetAllAsync();
		Task<ItemOrdeneDTO> GetByIdAsync(int id);
		Task AddAsync(ItemOrdeneDTO pedidoItem);
		Task UpdateAsync(ItemOrdeneDTO pedidoItem);
		Task DeleteAsync(int id);
	}
}
