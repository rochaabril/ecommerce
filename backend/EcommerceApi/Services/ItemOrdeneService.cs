using EcommerceApi.Dtos;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;

namespace EcommerceApi.Services
{
	public class ItemOrdeneService
	{
		private readonly IItemOrdeneRepository _pedidoItemRepository;

		public ItemOrdeneService(IItemOrdeneRepository pedidoItemRepository)
		{
			_pedidoItemRepository = pedidoItemRepository;
		}

		public Task<IEnumerable<ItemOrdeneDTO>> GetAllAsync() => _pedidoItemRepository.GetAllAsync();
		public Task<ItemOrdeneDTO> GetByIdAsync(int id) => _pedidoItemRepository.GetByIdAsync(id);
		public Task AddAsync(ItemOrdeneDTO pedidoItem) => _pedidoItemRepository.AddAsync(pedidoItem);
		public Task UpdateAsync(ItemOrdeneDTO pedidoItem) => _pedidoItemRepository.UpdateAsync(pedidoItem);
		public Task DeleteAsync(int id) => _pedidoItemRepository.DeleteAsync(id);

	}
}
