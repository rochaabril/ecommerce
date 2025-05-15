using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
    public static class CarritoMapper
    {
		public static CarritoDTO ToDTO(this Carrito carrito)
		{
			return new CarritoDTO
			{
				Id = carrito.Id,
				UsuarioId = carrito.UsuarioId,
			};
		}

		public static Carrito ToEntity(this CarritoDTO carritoDTO)
		{
			return new Carrito
			{
				Id = carritoDTO.Id,
				UsuarioId = carritoDTO.UsuarioId,
				
			};
		}
	}
}
