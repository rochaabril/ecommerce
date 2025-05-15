using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
    public static class OrdeneMapper
    {
        public static OrdeneDTO ToDTO(this Ordene ordene)
        {
            return new OrdeneDTO
            {
                Id = ordene.Id,
                UsuarioId = ordene.UsuarioId,
                Status=ordene.Status,
                Fecha = ordene.Fecha,
                Total = ordene.Total,
            };
                
        }

		public static Ordene ToEntity(this OrdeneDTO ordeneDTO)
		{
			return new Ordene
			{
				Id = ordeneDTO.Id,
				UsuarioId = ordeneDTO.UsuarioId,
				Fecha = ordeneDTO.Fecha,
				Total = ordeneDTO.Total,
                Status=ordeneDTO.Status,    
			};

		}


	}
}
