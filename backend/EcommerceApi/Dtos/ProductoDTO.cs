namespace EcommerceApi.Dtos
{
    public class ProductoDTO
    {
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;
		public decimal Precio { get; set; }
		public string? ImagenUrl { get; set; }
		public string? Observaciones { get; set; }
		public bool? Stock { get; set; }
		public int? CantidadMinima { get; set; }
		public int CategoriaId { get; set; }


	}



}
