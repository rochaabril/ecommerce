namespace EcommerceApi.Dtos
{
    public class OrdeneDTO
    {
		public int Id { get; set; }
		public int UsuarioId { get; set; }
		public decimal Total { get; set; }
		public string? Status { get; set; }
		public DateTime? Fecha { get; set; }
	}
}
