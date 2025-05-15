using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        [HttpGet("images/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var image = System.IO.File.OpenRead(path);
            return File(image, "image/jpeg");
        }
    }
}
