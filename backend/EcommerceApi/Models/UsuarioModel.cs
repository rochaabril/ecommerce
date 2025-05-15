using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class UsuarioModel
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public int RoleId { get; set; }
		public string? Direccion { get; set; }
		public string? Whatsapp { get; set; }
		public bool EmailVerificado { get; set; }
		public string? RememberToken { get; set; }

		public virtual Role Role { get; set; } = null!;
		public virtual List<CarritoModel> Carritos { get; set; }
		public virtual List<OrdeneModel> Ordenes { get; set; }

	}
}
