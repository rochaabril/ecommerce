using EcommerceApi.Dtos;
using EcommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdeneController : ControllerBase
    {
        private readonly OrdeneService _pedidoService;


		private readonly HttpClient _httpClient;
		private readonly string _telegramToken = "7550157178:AAFCEzQIOQzE4ycMc43UB1il2FwqXgbRPHU"; // Reemplaza con tu token
		private readonly string _chatId = "6178783111"; // Reemplaza con tu chat ID

		public OrdeneController(OrdeneService pedidoService,HttpClient httpClient)
        {
            _pedidoService = pedidoService;
            _httpClient= httpClient;   
        }

        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAll()
        {
            var pedido = await _pedidoService.GetAllAsync();
            return Ok(pedido);
        }

        [HttpGet("{id}")]
		[Authorize(Roles = "Admin,Cliente")]

		public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _pedidoService.GetByIdAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }
            return Ok(pedido);
        }

        [HttpPost]
		[Authorize(Roles = "Admin,Cliente")]

		public async Task<IActionResult> Create([FromBody] OrdeneDTO pedidoDTO)
        {
			//await _pedidoService.AddAsync(pedidoDTO);
			//return CreatedAtAction(nameof(GetById), new { id = pedidoDTO.Id }, pedidoDTO);
			await _pedidoService.AddAsync(pedidoDTO);
			await SendTelegramNotification(pedidoDTO);
			return CreatedAtAction(nameof(GetById), new { id = pedidoDTO.Id }, pedidoDTO);
		}

		private async Task SendTelegramNotification(OrdeneDTO pedidoDTO)
		{
			var message = $"Nueva orden generada:\n" +
						  $"Cliente: {pedidoDTO.UsuarioId}\n" +
						  $"Items:\n";

			

			var url = $"https://api.telegram.org/bot{_telegramToken}/sendMessage";
			var payload = new
			{
				chat_id = _chatId,
				text = message
			};

			var json = JsonSerializer.Serialize(payload);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			await _httpClient.PostAsync(url, content);
		}

		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Update(int id, [FromBody] OrdeneDTO pedidoDTO)
        {
            if (id != pedidoDTO.Id)
            {
                return BadRequest();
            }

            await _pedidoService.UpdateAsync(pedidoDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> Delete(int id)
        {
            await _pedidoService.DeleteAsync(id);
            return NoContent();
        }
    }
}
