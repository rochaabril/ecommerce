using EcommerceApi.Dtos;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;

namespace EcommerceApi.Services
{
	public class ItemCarritoService
	{
		private readonly IItemCarritoRepository _carritoItemRepository;

		public ItemCarritoService(IItemCarritoRepository carritoItemRepository)
		{
			_carritoItemRepository = carritoItemRepository;
		}

		public Task<IEnumerable<ItemCarritoDTO>> GetAllAsync() => _carritoItemRepository.GetAllAsync();
		public Task<ItemCarritoDTO> GetByIdAsync(int id) => _carritoItemRepository.GetByIdAsync(id);
        public Task AddAsync(ItemCarritoCompletoDTO carritoItem) => _carritoItemRepository.AddAsync(carritoItem);
		public Task UpdateAsync(ItemCarritoDTO carritoItem) => _carritoItemRepository.UpdateAsync(carritoItem);
		public Task DeleteAsync(int id) => _carritoItemRepository.DeleteAsync(id);
		public Task<IEnumerable<ItemCarritoDTO>> GetByCarritoIdAsync(int id) => _carritoItemRepository.GetByCarritoIdAsync(id);
        public async Task<ItemCarritoDTO> UpdateCantidadAsync(int idItem, int cantidad, string obser)
        {
            return await _carritoItemRepository.UpdateCantidad(idItem, cantidad, obser);
        }

    }
}
