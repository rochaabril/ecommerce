using EcommerceApi.Dtos;
using EcommerceApi.Models;
using EcommerceApi.Repositories.Interfaces;

namespace EcommerceApi.Services
{
    public class ProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public Task<IEnumerable<ProductoDTO>> GetAllAsync() => _productoRepository.GetAllAsync();
        public Task<ProductoDTO> GetByIdAsync(int id) => _productoRepository.GetByIdAsync(id);
        public Task AddAsync(ProductoDTO producto) => _productoRepository.AddAsync(producto);
        public Task UpdateAsync(ProductoDTO producto) => _productoRepository.UpdateAsync(producto);
        public Task DeleteAsync(int id) => _productoRepository.DeleteAsync(id);
		public Task<IEnumerable<ProductoDTO>> GetLatest10Async() => _productoRepository.GetLatest10Async();
	}

}
