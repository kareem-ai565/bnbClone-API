using bnbClone_API.Models;

namespace bnbClone_API.DTOs
{
    public class PropertyAmenityDTO
    {
        public int PropertyId { get; set; }
      
        public int AmenityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
