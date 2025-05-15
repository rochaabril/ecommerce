using System;
using System.Collections.Generic;

namespace EcommerceApi.Data
{
    public partial class Carrito
    {
        public Carrito()
        {
            ItemCarritos = new HashSet<ItemCarrito>();
        }

        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual ICollection<ItemCarrito> ItemCarritos { get; set; }
    }
}
