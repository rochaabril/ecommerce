using EcommerceApi.Dtos;

namespace EcommerceApi.Repositories.Interfaces
{
	public interface IRoleRepository
	{
		Task<IEnumerable<RoleDTO>> GetAllAsync();
		Task<RoleDTO> GetByIdAsync(int id);
		Task AddAsync(RoleDTO role);
		Task UpdateAsync(RoleDTO role);
		Task DeleteAsync(int id);
	}
}
