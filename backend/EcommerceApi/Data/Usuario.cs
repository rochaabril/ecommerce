using System;
using System.Collections.Generic;

namespace EcommerceApi.Data
{
    public partial class Usuario
    {
        public Usuario()
        {
            Carritos = new HashSet<Carrito>();
            Ordenes = new HashSet<Ordene>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string? Direccion { get; set; }
        public string? Whatsapp { get; set; }
        public bool EmailVerificado { get; set; }
        public string? RememberToken { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual ICollection<Carrito> Carritos { get; set; }
        public virtual ICollection<Ordene> Ordenes { get; set; }
    }
}
