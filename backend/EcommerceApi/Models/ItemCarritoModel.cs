using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class ItemCarritoModel
	{
		public int Id { get; set; }
		public int CarritoId { get; set; }
		public int ProductoId { get; set; }
		public int Cantidad { get; set; }
        public string? Observaciones { get; set; }
        public virtual CarritoModel Carrito { get; set; } = null!;
		public virtual ProductoModel Producto { get; set; } = null!;
	}
}
