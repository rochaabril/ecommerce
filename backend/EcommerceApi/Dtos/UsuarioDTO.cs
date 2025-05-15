namespace EcommerceApi.Dtos
{
    public class UsuarioDTO
    {
		public int Id { get; set; }
		public string Nombre { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
		public int RoleId { get; set; }
        public string? RoleName { get; set; }  // Nueva propiedad

        public string? Direccion { get; set; }
		public string? Whatsapp { get; set; }
		public bool EmailVerificado { get; set; }
		public string? RememberToken { get; set; }
	}
}
