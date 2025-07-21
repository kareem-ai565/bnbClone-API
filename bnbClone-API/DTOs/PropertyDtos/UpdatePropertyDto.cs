namespace bnbClone_API.DTOs.PropertyDtos
{
    public class UpdatePropertyDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxGuests { get; set; }
        public int NumOfBedrooms { get; set; }
        public int NumOfBathrooms { get; set; }
        public string Address { get; set; }

        public int PropertyTypeId { get; set; }
        public int HostId { get; set; }

        public List<int> AmenityIds { get; set; } = new(); // For many-to-many relation
    }

}
