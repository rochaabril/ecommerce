using EcommerceApi.Dtos;

namespace EcommerceApi.Repositories.Interfaces
{
	public interface ICarritoRepository
	{
		Task<IEnumerable<CarritoDTO>> GetAllAsync();
		Task<CarritoDTO> GetByIdAsync(int id);

		Task AddAsync(CarritoDTO carrito);
		Task UpdateAsync(CarritoDTO carrito);
		Task DeleteAsync(int id);
		Task<CarritoDTO> GetByUsuarioIdAsync(int usuarioId);
	}
}
