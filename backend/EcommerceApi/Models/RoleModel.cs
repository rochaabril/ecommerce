using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class RoleModel
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;

		public virtual List<UsuarioModel> Usuarios { get; set; }
	}
}
