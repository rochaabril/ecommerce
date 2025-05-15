namespace EcommerceApi.Dtos
{
    public class ItemCarritoCompletoDTO
    {
		public int Id { get; set; }
		public int CarritoId { get; set; }
		public int ProductoId { get; set; }
		public int Cantidad { get; set; }
        public string? Observaciones { get; set; }

    }

	public class ItemCarritoDTO
	{
		public int Id { get; set; }
		public int CarritoId { get; set; }
		public int ProductoId { get; set; }
		public int Cantidad { get; set; }
        public string? Observaciones { get; set; }
        public ProductoDTO? Producto { get; set; }

	}
}
