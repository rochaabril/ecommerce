using System;
using System.Collections.Generic;

namespace EcommerceApi.Data
{
    public partial class Ordene
    {
        public Ordene()
        {
            ItemOrdenes = new HashSet<ItemOrdene>();
        }

        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Total { get; set; }
        public string? Status { get; set; }
        public DateTime? Fecha { get; set; }

        public virtual Usuario Usuario { get; set; } = null!;
        public virtual ICollection<ItemOrdene> ItemOrdenes { get; set; }
    }
}
