using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bnbClone_API.DTOs
{
    public class AmenityDTO
    {

        public string Name { get; set; } = string.Empty;
        
        public string? Category { get; set; }
        
        public IFormFile IconUrl { get; set; }

    }
}
