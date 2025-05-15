using EcommerceApi.Dtos;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using EcommerceApi.Models;

namespace EcommerceApi.Services
{
	public class CarritoService
	{
		private readonly ICarritoRepository _carritoRepository;
		private readonly IItemCarritoRepository _itemCarritoRepository;
		private readonly OrdeneService _ordeneService;
		private readonly UsuarioService _usuarioService; // Añadir servicio de usuario

		private readonly HttpClient _httpClient;
		private readonly string _telegramToken = "8033266796:AAHXdsdgJj9bERKY1frd4m5IcHeWvgE3ZcY"; // Reemplaza con tu token
		private readonly string _chatId = "8059641985"; // Reemplaza con tu chat ID




		public CarritoService(HttpClient httpClient, ICarritoRepository carritoRepository, IItemCarritoRepository itemCarritoRepository, OrdeneService ordeneService, UsuarioService usuarioService)
		{
			_carritoRepository = carritoRepository;
			_itemCarritoRepository = itemCarritoRepository;
			_ordeneService = ordeneService;
			_usuarioService = usuarioService;
			_httpClient = httpClient;
		}

		public Task<IEnumerable<CarritoDTO>> GetAllAsync() => _carritoRepository.GetAllAsync();
		public Task<CarritoDTO> GetByIdAsync(int id) => _carritoRepository.GetByIdAsync(id);
		public Task AddAsync(CarritoDTO carrito) => _carritoRepository.AddAsync(carrito);
		public Task UpdateAsync(CarritoDTO carrito) => _carritoRepository.UpdateAsync(carrito);
		public Task DeleteAsync(int id) => _carritoRepository.DeleteAsync(id);


		public async Task<CarritoDTO> GetByUsuarioIdAsync(int usuarioId)
		{
			// Llama al repositorio para obtener el carrito asociado al usuario
			return await _carritoRepository.GetByUsuarioIdAsync(usuarioId);
		}
		public async Task FinalizeCarritoAsync(int carritoId)
		{
			// Obtener el carrito
			var carrito = await _carritoRepository.GetByIdAsync(carritoId);
			if (carrito == null) throw new Exception("Carrito no encontrado.");

			// Obtener los ítems del carrito
			var items = await _itemCarritoRepository.GetByCarritoIdAsync(carritoId);
			if (!items.Any()) throw new Exception("El carrito está vacío.");

			// Crear una nueva orden
			var nuevaOrden = new OrdeneDTO
			{
				UsuarioId = carrito.UsuarioId,
				Fecha = DateTime.UtcNow,
				Total = items.Sum(i => i.Cantidad * i.Producto.Precio), // Calcula el total
				Status = "Pendiente"
			};

			// Obtener el usuario asociado al carrito
			var usuario = await _usuarioService.GetByIdAsync(carrito.UsuarioId); // Obtener usuario por UsuarioId
			if (usuario == null) throw new Exception("Usuario no encontrado.");

			// Guardar la orden
			await _ordeneService.AddAsync(nuevaOrden);

			// Limpiar los ítems del carrito
			foreach (var item in items)
			{
				await _itemCarritoRepository.DeleteAsync(item.Id);
			}
			await SendTelegramNotification(nuevaOrden,items,usuario);
			

		}


		private async Task SendTelegramNotification(OrdeneDTO pedidoDTO, IEnumerable<ItemCarritoDTO> items, UsuarioDTO usuario)
		{
			if (_httpClient == null)
			{
				throw new InvalidOperationException("HttpClient no ha sido inicializado correctamente.");
			}

			var message = $"Nueva orden generada:\n" +
						  $"Cliente: {pedidoDTO.UsuarioId}\n" +
						  $"Nombre: {usuario.Nombre}\n" +
						  $"Whatsapp: {usuario.Whatsapp}\n" +
						  $"Email: {usuario.Email}\n" +
						  $"Total Compra: {pedidoDTO.Total:C}\n\n";

			foreach (var item in items)
			{
				message += $"Producto: {item.Producto.Nombre}\nCantidad: {item.Cantidad}\n" +
						   $"Precio Unitario: {item.Producto.Precio:C}\n" +
						   $"Subtotal: {item.Cantidad * item.Producto.Precio:C}\n\n";
			}

			try
			{
				var url = $"https://api.telegram.org/bot{_telegramToken}/sendMessage";
				var payload = JsonSerializer.Serialize(new { chat_id = _chatId, text = message });
				var content = new StringContent(payload, Encoding.UTF8, "application/json");

				var response = await _httpClient.PostAsync(url, content);

				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					throw new Exception($"Error al enviar notificación: {response.StatusCode}, Detalles: {errorContent}");
				}
			}
			catch (Exception ex)
			{
				// Agregar logging para depuración
				Console.WriteLine($"Fallo al enviar mensaje por Telegram: {ex.Message}");
				throw new Exception($"Fallo al enviar mensaje por Telegram: {ex.Message}", ex);
			}
		}
	}
}
