using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
	public class ItemCarritoRepository:IItemCarritoRepository
	{
		private readonly tienda_mayoristaContext _context;

		public ItemCarritoRepository(tienda_mayoristaContext context)
		{
			_context = context;
		}

		public async Task AddAsync(ItemCarritoCompletoDTO itemCarritoDTO)
		{
			var carritoItem = itemCarritoDTO.ToEntityComplete();  // Mapea DTO a entidad
			_context.ItemCarritos.Add(carritoItem);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var carritoItem = await _context.ItemCarritos.FindAsync(id);
			if (carritoItem != null)
			{
				_context.ItemCarritos.Remove(carritoItem);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<ItemCarritoDTO>> GetAllAsync()
		{
			var carritoItems = await _context.ItemCarritos.ToListAsync();
			return carritoItems.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
		}

		public async Task<ItemCarritoDTO> GetByIdAsync(int id)
		{
			var carritoItem = await _context.ItemCarritos.FindAsync(id);
			return carritoItem?.ToDTO();  // Mapea entidad a DTO
		}

		public async Task UpdateAsync(ItemCarritoDTO itemCarritoDTO)
		{
			var itemCarritoSelect = itemCarritoDTO.ToEntity();  // Mapea DTO a entidad
			_context.ItemCarritos.Update(itemCarritoSelect);
			await _context.SaveChangesAsync();
		}



        public async Task<IEnumerable<ItemCarritoDTO>> GetByCarritoIdAsync(int carritoId)
        {
            var items = await _context.ItemCarritos
                .Where(ic => ic.CarritoId == carritoId)
                .Include(ic => ic.Producto)  // Incluir la información del producto
                .ToListAsync();

            // Depuración para verificar si los productos están siendo cargados
            foreach (var item in items)
            {
                if (item.Producto == null)
                {
                    Console.WriteLine($"Producto no encontrado para ItemCarrito con ID: {item.Id}");
                }
                else
                {
                    Console.WriteLine($"Producto cargado: {item.Producto.Nombre}");
                }
            }

            return items.Select(ic => new ItemCarritoDTO
            {
                Id = ic.Id,
                CarritoId = ic.CarritoId,
                ProductoId = ic.ProductoId,
                Cantidad = ic.Cantidad,
                Observaciones = ic.Observaciones,
                Producto = ic.Producto != null
                    ? new ProductoDTO
                    {
                        Id = ic.Producto.Id,
                        Nombre = ic.Producto.Nombre,
                        Precio = ic.Producto.Precio,
                        CantidadMinima = ic.Producto.CantidadMinima,
                    }
                    : null
            });
        }

        public async Task<ItemCarritoDTO> UpdateCantidad(int idItem, int cantidad, string obser)
        {
            var itemCarrito = await _context.ItemCarritos.FindAsync(idItem);


            // Actualizar la cantidad
            itemCarrito.Cantidad = cantidad;
            itemCarrito.Observaciones = obser;

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Retornar el DTO con la información actualizada
            return new ItemCarritoDTO
            {
                Id = itemCarrito.Id,
                CarritoId = itemCarrito.CarritoId,
                ProductoId = itemCarrito.ProductoId,
                Cantidad = itemCarrito.Cantidad,
                Observaciones = itemCarrito.Observaciones,
                Producto = itemCarrito.Producto != null
                    ? new ProductoDTO
                    {
                        Id = itemCarrito.Producto.Id,
                        Nombre = itemCarrito.Producto.Nombre,
                        Precio = itemCarrito.Producto.Precio,
                        CantidadMinima = itemCarrito.Producto.CantidadMinima,
                    }
                    : null
            };
        }
    }
}
