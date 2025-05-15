using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
    public static class CategoriaMapper
    {
        public static CategoriaDTO ToDTO(this Categoria categoria)
        {
            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

		public static Categoria ToEntity(this CategoriaDTO categoriaDTO)
		{
			return new Categoria
			{
				Id = categoriaDTO.Id,
				Nombre = categoriaDTO.Nombre
			};
		}
	}

}
