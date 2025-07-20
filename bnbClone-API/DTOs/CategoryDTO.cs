using System.Text.Json.Serialization;

namespace bnbClone_API.DTOs
{
    public class CategoryDTO
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public IFormFile IconUrl { get; set; }

    }
}
