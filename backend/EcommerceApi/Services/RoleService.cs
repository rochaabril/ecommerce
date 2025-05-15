using EcommerceApi.Dtos;
using EcommerceApi.Repositories.Interfaces;

namespace EcommerceApi.Services
{
	public class RoleService
	{
		private readonly IRoleRepository _roleRepository;

		public RoleService(IRoleRepository roleRepository)
		{
			_roleRepository = roleRepository;
		}

		public Task<IEnumerable<RoleDTO>> GetAllAsync() => _roleRepository.GetAllAsync();
		public Task<RoleDTO> GetByIdAsync(int id) => _roleRepository.GetByIdAsync(id);
		public Task AddAsync(RoleDTO role) => _roleRepository.AddAsync(role);
		public Task UpdateAsync(RoleDTO role) => _roleRepository.UpdateAsync(role);
		public Task DeleteAsync(int id) => _roleRepository.DeleteAsync(id);
	}
}
