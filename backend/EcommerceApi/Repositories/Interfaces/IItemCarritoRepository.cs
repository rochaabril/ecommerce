using EcommerceApi.Dtos;

namespace EcommerceApi.Repositories.Interfaces
{
	public interface IItemCarritoRepository
	{
		Task<IEnumerable<ItemCarritoDTO>> GetAllAsync();
		Task<ItemCarritoDTO> GetByIdAsync(int id);

		Task AddAsync(ItemCarritoCompletoDTO carritoItem);
		Task UpdateAsync(ItemCarritoDTO carritoItem);
		Task<ItemCarritoDTO> UpdateCantidad(int idItem,int cantiadad, string obser);
		Task DeleteAsync(int id);

		Task<IEnumerable<ItemCarritoDTO>> GetByCarritoIdAsync(int carritoId); // Ítems por carrito

	}
}
