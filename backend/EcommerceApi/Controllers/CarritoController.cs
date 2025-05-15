using EcommerceApi.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly CarritoService _carritoService;

        public CarritoController(CarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        [HttpGet]
		public async Task<IActionResult> GetAll()
        {
            var carritos = await _carritoService.GetAllAsync();
            return Ok(carritos);
        }

        [HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
        {
            var carrito = await _carritoService.GetByIdAsync(id);
            if (carrito == null)
            {
                return NotFound();
            }
            return Ok(carrito);
        }

        [HttpPost]
		public async Task<IActionResult> Create([FromBody] CarritoDTO carritoDTO)
        {
            await _carritoService.AddAsync(carritoDTO);
            return CreatedAtAction(nameof(GetById), new { id = carritoDTO.Id }, carritoDTO);
        }

        [HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, [FromBody] CarritoDTO carritoDTO)
        {
            if (id != carritoDTO.Id)
            {
                return BadRequest();
            }

            await _carritoService.UpdateAsync(carritoDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
        {
            await _carritoService.DeleteAsync(id);
            return NoContent();
        }

		//////////////////////
		///

		[HttpPost("finalizar/{carritoId}")]
		public async Task<IActionResult> FinalizarCarrito(int carritoId)
		{
			try
			{
				await _carritoService.FinalizeCarritoAsync(carritoId);
				return Ok("Carrito finalizado y orden creada.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
		[HttpGet("usuario/{usuarioId}")]
		public async Task<IActionResult> GetByUsuarioId(int usuarioId)
		{
			try
			{
				var carrito = await _carritoService.GetByUsuarioIdAsync(usuarioId);
				if (carrito == null)
				{
					return NotFound("No se encontró un carrito para este usuario.");
				}
				return Ok(carrito);
			}
			catch (Exception ex)
			{
				return BadRequest($"Error al obtener el carrito del usuario: {ex.Message}");
			}
		}
	}
}
