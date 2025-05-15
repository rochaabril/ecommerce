using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class ItemOrdeneModel
	{
		public int Id { get; set; }
		public int OrdenId { get; set; }
		public int ProductoId { get; set; }
		public int Cantidad { get; set; }
		public decimal Total { get; set; }

		public virtual OrdeneModel Orden { get; set; } = null!;
		public virtual ProductoModel Producto { get; set; } = null!;
	}
}
