namespace EcommerceApi.Dtos
{
    public class ItemOrdeneDTO
    {
		public int Id { get; set; }
		public int OrdenId { get; set; }
		public int ProductoId { get; set; }
		public int Cantidad { get; set; }
		public decimal Total { get; set; }
	}
}
