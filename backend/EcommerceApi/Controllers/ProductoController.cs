using EcommerceApi.Dtos;
using EcommerceApi.Models;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace EcommerceApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoService _productoService;
		private readonly string _connectionString = "Server=sqlserver;Database=tienda_mayorista;User Id = Sa; Password=YourStrong@Passw0rd;";


		public ProductoController(ProductoService productoService)
        {
            _productoService = productoService;
        }
		
		[HttpPost("ActualizarPrecios")]
		public async Task<IActionResult> ActualizarPrecios([FromBody] ActualizarPreciosRequest request)
		{
            if (request.Porcentaje == 0)
            {
                return BadRequest("El porcentaje no puede ser 0.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Actualizar precios de los productos con redondeo hacia arriba
                    string updateProductosQuery = "UPDATE Productos SET Precio = CEILING(Precio * (1 + @Porcentaje / 100))";
                    using (SqlCommand updateProductosCommand = new SqlCommand(updateProductosQuery, connection))
                    {
                        updateProductosCommand.Parameters.AddWithValue("@Porcentaje", request.Porcentaje);
                        await updateProductosCommand.ExecuteNonQueryAsync();
                    }

                    // Eliminar todos los productos del carrito
                    string deleteCarritoQuery = "DELETE FROM Item_Carrito";
                    using (SqlCommand deleteCarritoCommand = new SqlCommand(deleteCarritoQuery, connection))
                    {
                        await deleteCarritoCommand.ExecuteNonQueryAsync();
                    }

                    return Ok(new { Message = "Precios actualizados, redondeados hacia arriba y carritos vaciados." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al actualizar los precios.", Error = ex.Message });
            }
        }


		public class ActualizarPreciosRequest
		{
			public decimal Porcentaje { get; set; }
		}









		[HttpGet]
		public async Task<IActionResult> GetAll()
        {
            var productos = await _productoService.GetAllAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _productoService.GetByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }
		[HttpGet("novedades")]
		public async Task<IActionResult> GetLatestProducts()
		{
			var productos = await _productoService.GetLatest10Async();
			return Ok(productos);
		}
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromForm] ProductoDTO productoDTO, IFormFile? archivo)
		{
			if (archivo != null && archivo.Length > 0)
			{
				var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

				if (!Directory.Exists(imagesFolder))
				{
					Directory.CreateDirectory(imagesFolder);
				}

				var fileName = Path.GetFileName(archivo.FileName);
				var filePath = Path.Combine(imagesFolder, fileName);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await archivo.CopyToAsync(stream);
				}

				productoDTO.ImagenUrl = $"/images/{fileName}";
			}

			// Guarda el producto y espera a que se complete
			await _productoService.AddAsync(productoDTO);

			// Aquí asumimos que el ID se ha generado y asignado al objeto productoDTO
			return CreatedAtAction(nameof(GetById), new { id = productoDTO.Id }, productoDTO);
		}

        //      [HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> Create([FromForm] ProductoDTO productoDTO,IFormFile archivo)
        //      {
        //	//await _productoService.AddAsync(productoDTO);
        //	//return CreatedAtAction(nameof(GetById), new { id = productoDTO.Id }, productoDTO);
        //	if (archivo == null || archivo.Length == 0)
        //	{
        //		return BadRequest("Se requiere una imagen para el producto.");
        //	}

        //	// Procesar la imagen (guardar en un directorio y obtener la URL)
        //	var rutaImagen = Path.Combine("wwwroot/images", archivo.FileName);
        //	using (var stream = new FileStream(rutaImagen, FileMode.Create))
        //	{
        //		await archivo.CopyToAsync(stream);
        //	}

        //	productoDTO.ImagenUrl = $"images/{archivo.FileName}"; // Asigna la URL de la imagen

        //	await _productoService.AddAsync(productoDTO);
        //	return CreatedAtAction(nameof(GetById), new { id = productoDTO.Id }, productoDTO);
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductoDTO productoDTO, IFormFile? archivo)
        {
            if (id != productoDTO.Id)
            {
                return BadRequest();
            }

            // Obtener el producto actual desde la base de datos
            var productoActual = await _productoService.GetByIdAsync(id);
            if (productoActual == null)
            {
                return NotFound("El producto no existe.");
            }

            // Procesar la imagen si se proporciona
            if (archivo != null && archivo.Length > 0)
            {
                // Ruta de la carpeta de imágenes
                var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Crear el directorio si no existe
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                // Eliminar la imagen anterior si existe
                if (!string.IsNullOrEmpty(productoActual.ImagenUrl))
                {
                    var rutaImagenAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productoActual.ImagenUrl.TrimStart('/'));
                    if (System.IO.File.Exists(rutaImagenAnterior))
                    {
                        System.IO.File.Delete(rutaImagenAnterior);
                    }
                }

                // Guardar la nueva imagen
                var fileName = Path.GetFileName(archivo.FileName);
                var filePath = Path.Combine(imagesFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await archivo.CopyToAsync(stream);
                }

                // Actualizar la URL de la imagen
                productoDTO.ImagenUrl = $"/images/{fileName}";
            }
            else
            {
                // Mantener la URL de la imagen existente si no se sube una nueva
                productoDTO.ImagenUrl = productoActual.ImagenUrl;
            }

            // Actualizar el producto
            await _productoService.UpdateAsync(productoDTO);

            return NoContent();
        }

        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Delete(int id)
        {
            await _productoService.DeleteAsync(id);
            return NoContent();
        }
    }

}
