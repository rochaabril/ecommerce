using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Models;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly tienda_mayoristaContext _context;

        public ProductoRepository(tienda_mayoristaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync()
        {
            var productos = await _context.Productos.ToListAsync();
            return productos.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
        }

        public async Task<ProductoDTO> GetByIdAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            return producto?.ToDTO();  // Mapea entidad a DTO
        }

        public async Task AddAsync(ProductoDTO productoDTO)
        {
            var producto = productoDTO.ToEntity();  // Mapea DTO a entidad
                                                    // Redondear el precio hacia arriba antes de agregar el producto
            producto.Precio = Math.Ceiling(producto.Precio);
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

			productoDTO.Id = producto.Id;
		}

        public async Task UpdateAsync(ProductoDTO productoDTO)
        {
            // Buscar el producto existente en la base de datos
            var productoExistente = await _context.Productos.FindAsync(productoDTO.Id);

            if (productoExistente == null)
            {
                throw new KeyNotFoundException("El producto no existe.");
            }

            // Mapear las propiedades del DTO a la entidad existente
            productoExistente.Nombre = productoDTO.Nombre;
            productoExistente.Precio = Math.Ceiling(productoDTO.Precio); // Redondear hacia arriba
            productoExistente.CategoriaId = productoDTO.CategoriaId;
            productoExistente.ImagenUrl = productoDTO.ImagenUrl;
            productoExistente.Stock = productoDTO.Stock;
            productoExistente.CantidadMinima = productoDTO.CantidadMinima;
            // Mapea otras propiedades según corresponda...

            // Guardar cambios
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
        }

		public async Task<IEnumerable<ProductoDTO>> GetLatest10Async()
		{
			var productos = await _context.Productos
	 .OrderByDescending(p => p.Id) // Asegúrate de usar el campo que determina el orden de creación
	 .Take(10)
	 .ToListAsync();

			return productos.Select(p => p.ToDTO()).ToList();
		}
	}

}
