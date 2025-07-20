using bnbClone_API.DTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> UploadImage(int id, [FromBody] CreatePropertyImageDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _propertyImageService.AddImageAsync(id, dto);
            return CreatedAtAction(nameof(GetImagesByPropertyId), new { id = id }, result);
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
