using System;
using System.Collections.Generic;

namespace EcommerceApi.Data
{
    public partial class Producto
    {
        public Producto()
        {
            ItemCarritos = new HashSet<ItemCarrito>();
            ItemOrdenes = new HashSet<ItemOrdene>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public string? Observaciones { get; set; }
        public bool? Stock { get; set; }
        public int? CantidadMinima { get; set; }
        public int CategoriaId { get; set; }

        public virtual Categoria Categoria { get; set; } = null!;
        public virtual ICollection<ItemCarrito> ItemCarritos { get; set; }
        public virtual ICollection<ItemOrdene> ItemOrdenes { get; set; }
    }
}
