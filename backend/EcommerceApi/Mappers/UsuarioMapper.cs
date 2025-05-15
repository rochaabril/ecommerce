using EcommerceApi.Data;
using EcommerceApi.Dtos;

namespace EcommerceApi.Mappers
{
    public static class UsuarioMapper
    {
        public static UsuarioDTO ToDTO(this Usuario usuario )
        {
            return new UsuarioDTO
            {
                Id= usuario.Id,
                Nombre=usuario.Nombre,
                Email=usuario.Email,
                Password=usuario.Password,
                RoleId=usuario.RoleId,
                Direccion=usuario.Direccion,
                Whatsapp=usuario.Whatsapp,
                EmailVerificado=usuario.EmailVerificado,
                RememberToken=usuario.RememberToken,    

            };
        }

        public static Usuario ToEntity(this UsuarioDTO usuarioDTO)
        {
            return new Usuario
            {
                Id= usuarioDTO.Id,  
                Nombre= usuarioDTO.Nombre,  
                Email=usuarioDTO.Email,
                Password=usuarioDTO.Password,
                RoleId = usuarioDTO.RoleId,
                Direccion=usuarioDTO.Direccion,
                Whatsapp=usuarioDTO.Whatsapp,
                EmailVerificado=usuarioDTO.EmailVerificado,
                RememberToken=usuarioDTO.RememberToken,
                
                

            };
        }

    }
}
