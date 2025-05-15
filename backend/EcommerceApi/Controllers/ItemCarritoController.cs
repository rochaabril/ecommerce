using EcommerceApi.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCarritoController : ControllerBase
    {
        private readonly ItemCarritoService _carritoItemService;

        public ItemCarritoController(ItemCarritoService carritoItemService)
        {
            _carritoItemService = carritoItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var carritoItems = await _carritoItemService.GetAllAsync();
            return Ok(carritoItems);
        }

        [HttpGet("idCarrito/{idCarrito}")]
        public async Task<IActionResult> GetByIdCarrito(int idCarrito)
        {
            var carritoItem = await _carritoItemService.GetByCarritoIdAsync(idCarrito);
          
            return Ok(carritoItem);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var carritoItem = await _carritoItemService.GetByIdAsync(id);
            if (carritoItem == null)
            {
                return NotFound();
            }
            return Ok(carritoItem);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemCarritoCompletoDTO carritoItemDTO)
        {
            await _carritoItemService.AddAsync(carritoItemDTO);
            return CreatedAtAction(nameof(GetById), new { id = carritoItemDTO.Id }, carritoItemDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ItemCarritoDTO carritoItemDTO)
        {
            if (id != carritoItemDTO.Id)
            {
                return BadRequest();
            }

            await _carritoItemService.UpdateAsync(carritoItemDTO);
            return NoContent();
        }
        [HttpPut("updateCantidad/{idItem}")]
        public async Task<IActionResult> UpdateCantidad(int idItem, int cantidad, string obser)
        {
            var updatedItem = await _carritoItemService.UpdateCantidadAsync(idItem, cantidad, obser);

            if (updatedItem == null)
            {
                return NotFound();
            }

            return Ok(updatedItem);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _carritoItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
