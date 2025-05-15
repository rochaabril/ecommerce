using System;
using System.Collections.Generic;

namespace EcommerceApi.Data
{
    public partial class ItemOrdene
    {
        public int Id { get; set; }
        public int OrdenId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        public virtual Ordene Orden { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
}
