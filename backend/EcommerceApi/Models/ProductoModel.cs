using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class ProductoModel
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;
		public decimal Precio { get; set; }
		public string? ImagenUrl { get; set; }
		public string? Observaciones { get; set; }
		public bool? Stock { get; set; }
		public int? CantidadMinima { get; set; }
		public int CategoriaId { get; set; }
		public virtual Categoria Categoria { get; set; } = null!;

		public virtual List<ItemCarritoModel> ItemCarritos { get; set; }
		public virtual List<ItemOrdeneModel> ItemOrdenes { get; set; }

	}
}
