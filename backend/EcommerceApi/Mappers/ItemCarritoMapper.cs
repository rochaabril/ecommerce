using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
    public static class ItemCarritoMapper
    {
		public static ItemCarritoDTO ToDTO(this ItemCarrito itemCarrito)
		{
			return new ItemCarritoDTO
			{
				Id = itemCarrito.Id,
				CarritoId = itemCarrito.CarritoId,
				ProductoId= itemCarrito.ProductoId,
				Cantidad= itemCarrito.Cantidad,
				Observaciones= itemCarrito.Observaciones,
			};
		}

		public static ItemCarrito ToEntity(this ItemCarritoDTO itemCarritoDTO)
		{
			return new ItemCarrito
			{
				Id= itemCarritoDTO.Id,
				CarritoId= itemCarritoDTO.CarritoId,
				ProductoId= itemCarritoDTO.ProductoId,
				Cantidad=itemCarritoDTO.Cantidad,
				Observaciones = itemCarritoDTO.Observaciones,
			};
		}

		public static ItemCarrito ToEntityComplete(this ItemCarritoCompletoDTO itemCarritoDTO)
		{
			return new ItemCarrito
			{
				Id = itemCarritoDTO.Id,
				CarritoId = itemCarritoDTO.CarritoId,
				ProductoId = itemCarritoDTO.ProductoId,
				Cantidad = itemCarritoDTO.Cantidad,
				Observaciones = itemCarritoDTO.Observaciones	
			};
		}
	}
}
