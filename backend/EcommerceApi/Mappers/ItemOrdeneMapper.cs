using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
    public static class ItemOrdeneMapper
    {
		public static ItemOrdeneDTO ToDTO(this ItemOrdene itemOrdene)
		{
			return new ItemOrdeneDTO
			{
				Id = itemOrdene.Id,
				OrdenId = itemOrdene.OrdenId,
				ProductoId=itemOrdene.ProductoId,
				Cantidad=itemOrdene.Cantidad,
				Total=itemOrdene.Total,
			};
		}

		public static ItemOrdene ToEntity(this ItemOrdeneDTO itemOrdeneDTO)
		{
			return new ItemOrdene
			{
				Id = itemOrdeneDTO.Id,
				OrdenId=itemOrdeneDTO.OrdenId,
				ProductoId=itemOrdeneDTO.ProductoId,
				Cantidad=itemOrdeneDTO.Cantidad,
				Total=itemOrdeneDTO.Total
			};
		}


	}
}
