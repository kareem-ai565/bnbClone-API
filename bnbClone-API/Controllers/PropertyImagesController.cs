using bnbClone_API.DTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using bnbClone_API.DTOs.PropertyDtos;

namespace bnbClone_API.Controllers
{
    [ApiController]
    [Route("api")]
    public class PropertyImagesController : ControllerBase
    {
        private readonly IPropertyImageService _propertyImageService;

        public PropertyImagesController(IPropertyImageService propertyImageService)
        {
            _propertyImageService = propertyImageService;
        }

        // ✅ POST: /api/properties/{id}/images
        [HttpPost("properties/{id}/images")]
        public async Task<IActionResult> UploadImage(int id, IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("No image uploaded");

            var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine("wwwroot/images", fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var dto = new CreatePropertyImageDto
            {
                ImageUrl = fileName 
            };

            var result = await _propertyImageService.AddImageAsync(id, dto);
            return Ok(new { fileName }); 
        }



        // ✅ GET: /api/properties/{id}/images
        [HttpGet("properties/{id}/images")]
        public async Task<IActionResult> GetImagesByPropertyId(int id)
        {
            var images = await _propertyImageService.GetImagesByPropertyIdAsync(id);
            return Ok(images);
        }

        // ✅ DELETE: /api/property-images/{imageId}
        [HttpDelete("property-images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var success = await _propertyImageService.DeleteImageAsync(imageId);
            if (!success)
                return NotFound("Image not found");

            return Ok("Image deleted successfully.");
        }














    }
}
