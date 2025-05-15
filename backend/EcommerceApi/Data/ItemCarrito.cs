﻿using System;
using System.Collections.Generic;

namespace EcommerceApi.Data
{
    public partial class ItemCarrito
    {
        public int Id { get; set; }
        public int CarritoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public string? Observaciones { get; set; }

        public virtual Carrito Carrito { get; set; } = null!;
        public virtual Producto Producto { get; set; } = null!;
    }
}
