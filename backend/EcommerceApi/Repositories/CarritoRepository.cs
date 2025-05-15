using EcommerceApi.Data;
using EcommerceApi.Dtos;
using EcommerceApi.Mappers;
using EcommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Repositories
{
	public class CarritoRepository:ICarritoRepository
	{
		private readonly tienda_mayoristaContext _context;

		public CarritoRepository(tienda_mayoristaContext context)
		{
			_context = context;
		}

		public async Task AddAsync(CarritoDTO carrito)
		{
			var car = carrito.ToEntity();// Mapea DTO a entidad
			_context.Carritos.Add(car);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var carrito = await _context.Carritos.FindAsync(id);
			if (carrito != null)
			{
				_context.Carritos.Remove(carrito);
				await _context.SaveChangesAsync();
			}
		}

		public async Task<IEnumerable<CarritoDTO>> GetAllAsync()
		{
			var carritos = await _context.Carritos.ToListAsync();
			return carritos.Select(p => p.ToDTO()).ToList();  // Mapea entidades a DTOs
		}

		public async Task<CarritoDTO> GetByIdAsync(int id)
		{
			var carrito = await _context.Carritos.FindAsync(id);
			return carrito?.ToDTO();  // Mapea entidad a DTO
		}

		public async Task UpdateAsync(CarritoDTO carrito)
		{
			var carritoSelect = carrito.ToEntity();  // Mapea DTO a entidad
			_context.Carritos.Update(carritoSelect);
			await _context.SaveChangesAsync();
		}
		public async Task<CarritoDTO> GetByUsuarioIdAsync(int usuarioId)
		{
			// Suponiendo que tienes una tabla `Carrito` con una columna `UsuarioId`
			var carrito = await _context.Carritos
				.Where(c => c.UsuarioId == usuarioId)
				.FirstOrDefaultAsync();

			if (carrito == null) return null;

			// Mapear a DTO si es necesario
			return new CarritoDTO
			{
				Id = carrito.Id,
				UsuarioId = carrito.UsuarioId,
				// Otros campos según tu modelo
			};
		}
	}
}
