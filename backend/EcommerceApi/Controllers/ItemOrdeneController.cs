using EcommerceApi.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemOrdeneController : ControllerBase
    {
        private readonly ItemOrdeneService _pedidoItemService;

        public ItemOrdeneController(ItemOrdeneService pedidoItemService)
        {
            _pedidoItemService = pedidoItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {                                                                                     
            var personas = await _pedidoItemService.GetAllAsync();
            return Ok(personas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _pedidoItemService.GetByIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemOrdeneDTO pedidoItemDTO)
        {
            await _pedidoItemService.AddAsync(pedidoItemDTO);
            return CreatedAtAction(nameof(GetById), new { id = pedidoItemDTO.Id }, pedidoItemDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ItemOrdeneDTO pedidoItemDTO)
        {
            if (id != pedidoItemDTO.Id)
            {
                return BadRequest();
            }

            await _pedidoItemService.UpdateAsync(pedidoItemDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pedidoItemService.DeleteAsync(id);
            return NoContent();
        }
    }
}
