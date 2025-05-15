using EcommerceApi.Dtos;

namespace EcommerceApi.Repositories.Interfaces
{
	public interface IOrdeneRepository
	{
		Task<IEnumerable<OrdeneDTO>> GetAllAsync();
		Task<OrdeneDTO> GetByIdAsync(int id);
		Task AddAsync(OrdeneDTO pedido);
		Task UpdateAsync(OrdeneDTO pedido);
		Task DeleteAsync(int id);
	}
}
