using EcommerceApi.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _categoriaService.GetAllAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var categoria = await _categoriaService.GetByIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return Ok(categoria);
        }

        [HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Create([FromBody] CategoriaDTO categoriaDTO)
        {
            await _categoriaService.AddAsync(categoriaDTO);
            return CreatedAtAction(nameof(GetById), new { id = categoriaDTO.Id }, categoriaDTO);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoriaDTO categoriaDTO)
        {
            if (id != categoriaDTO.Id)
            {
                return BadRequest();
            }

            await _categoriaService.UpdateAsync(categoriaDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoriaService.DeleteAsync(id);
            return NoContent();
        }
    }
}
