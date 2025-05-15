using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Repositories.Interfaces
{
	public interface IUsuarioRepository
	{
		Task<IEnumerable<UsuarioDTO>> GetAllAsync();
		Task<UsuarioDTO> GetByIdAsync(int id);
		Task<UsuarioDTO> AddAsync(UsuarioDTO usuario);
		Task UpdateAsync(UsuarioDTO usuario);
		Task DeleteAsync(int id);
		Task<UsuarioDTO> GetByEmailAsync(string email);
		Task<string> GetRoleNameByIdAsync(int roleId);
		Task SaveResetTokenAsync(int userId, string token);
		Task<Usuario> GetByResetTokenAsync(string token);



	}
}
