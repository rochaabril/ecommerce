using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class OrdeneModel
	{
		public int Id { get; set; }
		public int UsuarioId { get; set; }
		public decimal Total { get; set; }
		public string? Status { get; set; }
		public DateTime Fecha { get; set; }

		public virtual UsuarioModel Usuario { get; set; } = null!;
		public virtual List<ItemOrdeneModel> ItemOrdenes { get; set; }
	}
}
