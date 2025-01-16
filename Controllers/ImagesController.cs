using ImageMS.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IGoogleCloudStorageService _storageService;
        public ImagesController(IGoogleCloudStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpPost("UploadImage/{folderName}")] // Provides easier access to the Endpoint
        public async Task<IActionResult> UploadImage(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Archivo No Válido");
            }

            try
            {
                var url = await _storageService.UploadImageAsync(file, folderName);

                // Return an HTTP 200 Code and a JSON object which contains the URL of the uploaded image
                return Ok(new { Url = url });
            }
            catch (Exception ex)
            {
                // Return an HTTP 500 Code if the image couldn't be uploaded
                return StatusCode(500, $"Error al subir la imagen: {ex.Message}");
            }
        }
    }
}
