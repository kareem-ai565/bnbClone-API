namespace bnbClone_API.DTOs.PropertyDtos
{
    public class UpdatePropertyDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? PricePerNight { get; set; }
        public int? MaxGuests { get; set; }

        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public string? PropertyType { get; set; }
        public int? HostId { get; set; }
    }
}
