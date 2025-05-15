using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class CarritoModel
	{
		public int Id { get; set; }
		public int UsuarioId { get; set; }

		public virtual Usuario Usuario { get; set; } = null!;
		public virtual List<ItemCarritoModel> ItemCarritos { get; set; }
	}
}
