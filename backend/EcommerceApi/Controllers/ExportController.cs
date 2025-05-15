using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using System.IO;

namespace EcommerceApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ExportController : ControllerBase
	{
		private readonly string _connectionString = "Server=sqlserver;Database=tienda_mayorista;User Id = Sa; Password=YourStrong@Passw0rd;";

		[HttpGet("ExportarProductos")]
		public async Task<IActionResult> ExportarProductos()
		{
			try
			{
				// Configurar la licencia de EPPlus
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				// Consulta SQL para obtener todos los productos con sus categorías
				var query = @"SELECT p.Id, p.Nombre, p.Precio, p.ImagenUrl, p.Stock, p.CantidadMinima, c.Nombre AS Categoria 
                                FROM Productos p
                                LEFT JOIN Categorias c ON p.CategoriaId = c.Id";

				DataTable productosTable = new DataTable();
				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					await connection.OpenAsync();
					using (SqlCommand command = new SqlCommand(query, connection))
					using (SqlDataAdapter adapter = new SqlDataAdapter(command))
					{
						adapter.Fill(productosTable);
					}
				}

				// Crear el archivo Excel
				using (var package = new ExcelPackage())
				{
					var worksheet = package.Workbook.Worksheets.Add("Productos");

					// Encabezados
					worksheet.Cells["A1"].Value = "ID";
					worksheet.Cells["B1"].Value = "Nombre";
					worksheet.Cells["C1"].Value = "Precio";
					worksheet.Cells["D1"].Value = "Imagen";
					worksheet.Cells["E1"].Value = "Stock";
					worksheet.Cells["F1"].Value = "Cantidad Mínima";
					worksheet.Cells["G1"].Value = "Categoría";

					worksheet.Row(1).Style.Font.Bold = true;

					// Ajustar altura de las filas para las imágenes
					worksheet.Row(1).Height = 50; // Altura de encabezado

					// Rellenar datos
					int row = 2;
					foreach (DataRow producto in productosTable.Rows)
					{
						worksheet.Cells[row, 1].Value = producto["Id"];
						worksheet.Cells[row, 2].Value = producto["Nombre"];
						worksheet.Cells[row, 3].Value = producto["Precio"];

						// Convertir Stock a "Sí" o "No"
						bool stockValue = producto["Stock"] != DBNull.Value && Convert.ToBoolean(producto["Stock"]);
						worksheet.Cells[row, 5].Value = stockValue ? "Sí" : "No";

						worksheet.Cells[row, 6].Value = producto["CantidadMinima"];
						worksheet.Cells[row, 7].Value = producto["Categoria"];

						// Manejar imagen
						var imagenUrl = producto["ImagenUrl"]?.ToString();
						if (!string.IsNullOrEmpty(imagenUrl))
						{
							var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagenUrl.TrimStart('/'));
							if (System.IO.File.Exists(imagePath))
							{
								using (var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
								{
									var picture = worksheet.Drawings.AddPicture($"Img_{row}", fs);
									picture.SetPosition(row - 1, 5, 3, 5); // Fila actual, columna D
									picture.SetSize(50, 50); // Tamaño de la imagen para ajustarse a la celda
								}
							}
							else
							{
								worksheet.Cells[row, 4].Value = "Imagen no encontrada"; // Si no existe, muestra texto
							}
						}
						else
						{
							worksheet.Cells[row, 4].Value = "Sin imagen";
						}

						// Ajustar altura de la fila
						worksheet.Row(row).Height = 50; // Altura para que la imagen quede alineada

						row++;
					}

					// Ajustar el tamaño de las columnas
					worksheet.Cells.AutoFitColumns();

					// Devolver el archivo Excel
					var excelBytes = package.GetAsByteArray();
					return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Productos.xlsx");
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, new { Message = "Ocurrió un error al exportar los productos.", Error = ex.Message });
			}
		}
	}
}