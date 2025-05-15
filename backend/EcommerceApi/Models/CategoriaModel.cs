using EcommerceApi.Data;

namespace EcommerceApi.Models
{
	public class CategoriaModel
	{
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;

		public virtual List<ProductoModel> Productos { get; set; }
	}
}
