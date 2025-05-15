using System;
using System.Collections.Generic;

namespace EcommerceApi.Data
{
    public partial class Categoria
    {
        public Categoria()
        {
            Productos = new HashSet<Producto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; }
    }
}
