using EcommerceApi.Dtos;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;

namespace EcommerceApi.Services
{
	public class OrdeneService
	{
        private readonly IOrdeneRepository _pedidoRepository;

        public OrdeneService(IOrdeneRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        public Task<IEnumerable<OrdeneDTO>> GetAllAsync() => _pedidoRepository.GetAllAsync();
        public Task<OrdeneDTO> GetByIdAsync(int id) => _pedidoRepository.GetByIdAsync(id);
        public Task AddAsync(OrdeneDTO pedido) => _pedidoRepository.AddAsync(pedido);
        public Task UpdateAsync(OrdeneDTO pedido) => _pedidoRepository.UpdateAsync(pedido);
        public Task DeleteAsync(int id) => _pedidoRepository.DeleteAsync(id);
    }
}
