using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Models;

namespace EcommerceApi.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<ProductoDTO>> GetAllAsync();
        Task<ProductoDTO> GetByIdAsync(int id);
		Task<IEnumerable<ProductoDTO>> GetLatest10Async();

		Task AddAsync(ProductoDTO producto);
        Task UpdateAsync(ProductoDTO producto);
        Task DeleteAsync(int id);
    }
}
