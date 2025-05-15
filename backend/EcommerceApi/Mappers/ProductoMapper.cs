using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
    public static class ProductoMapper
    {
        public static ProductoDTO ToDTO(this Producto producto)
        {
            return new ProductoDTO
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Precio = producto.Precio,
                ImagenUrl=producto.ImagenUrl,
                Observaciones=producto.Observaciones,
                Stock=producto.Stock,
                CantidadMinima=producto.CantidadMinima,
                CategoriaId=producto.CategoriaId,
                
                
            };
        }

        public static Producto ToEntity(this ProductoDTO productoDTO)
        {
            return new Producto
            {
                Nombre = productoDTO.Nombre,
                Precio = productoDTO.Precio,
                ImagenUrl=productoDTO.ImagenUrl,
                Observaciones=productoDTO.Observaciones,
                Stock = productoDTO.Stock,  
                CantidadMinima=productoDTO.CantidadMinima,
                CategoriaId=productoDTO.CategoriaId,
            };
        }
    }

}
