using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
	public static class RoleMapper
	{
		public static RoleDTO ToDTO(this Role role)
		{
			return new RoleDTO
			{
				Id= role.Id,
				Nombre=role.Nombre,	

			};
		}

		public static Role ToEntity(this RoleDTO roleDTO)
		{
			return new Role
			{
				Id=roleDTO.Id,
				Nombre=roleDTO.Nombre,

			};
		}
	}
}
