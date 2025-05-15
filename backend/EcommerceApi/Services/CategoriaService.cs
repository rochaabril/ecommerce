using EcommerceApi.Dtos;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;

namespace EcommerceApi.Services
{
	public class CategoriaService
	{
		private readonly ICategoriaRepository _categoriaRepository;

		public CategoriaService(ICategoriaRepository categoriaRepository)
		{
			_categoriaRepository = categoriaRepository;
		}

		public Task<IEnumerable<CategoriaDTO>> GetAllAsync() => _categoriaRepository.GetAllAsync();
		public Task<CategoriaDTO> GetByIdAsync(int id) => _categoriaRepository.GetByIdAsync(id);
		public Task AddAsync(CategoriaDTO categoria) => _categoriaRepository.AddAsync(categoria);
		public Task UpdateAsync(CategoriaDTO categoria) => _categoriaRepository.UpdateAsync(categoria);
		public Task DeleteAsync(int id) => _categoriaRepository.DeleteAsync(id);
	}
}
