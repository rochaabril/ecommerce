using EcommerceApi.Dtos;

namespace EcommerceApi.Repositories.Interfaces
{
	public interface ICategoriaRepository
	{
		Task<IEnumerable<CategoriaDTO>> GetAllAsync();
		Task<CategoriaDTO> GetByIdAsync(int id);
		Task AddAsync(CategoriaDTO categoria);
		Task UpdateAsync(CategoriaDTO categoria);
		Task DeleteAsync(int id);
	}
}
