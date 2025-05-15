using EcommerceApi.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RoleController : ControllerBase
	{
		private readonly RoleService _roleService;

		public RoleController(RoleService roleService)
		{
			_roleService = roleService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin,Cliente")]
		public async Task<IActionResult> GetAll()
		{
			var roles = await _roleService.GetAllAsync();
			return Ok(roles);
		}


		[HttpGet("{id}")]
		[Authorize(Roles = "Admin,Cliente")]
		public async Task<IActionResult> GetById(int id)
		{
			var orden = await _roleService.GetByIdAsync(id);
			if (orden == null)
			{
				return NotFound();
			}
			return Ok(orden);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Create([FromBody] RoleDTO roleDTO)
		{
			await _roleService.AddAsync(roleDTO);
			return CreatedAtAction(nameof(GetById), new { id = roleDTO.Id }, roleDTO);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Update(int id, [FromBody] RoleDTO roleDTO)
		{
			if (id != roleDTO.Id)
			{
				return BadRequest();
			}

			await _roleService.UpdateAsync(roleDTO);
			return NoContent();
		}

		[HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Delete(int id)
		{
			await _roleService.DeleteAsync(id);
			return NoContent();
		}


	}
}
