namespace bnbClone_API.DTOs
{
    public class PropertyDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal PricePerNight { get; set; }
        public int MaxGuests { get; set; }
        public int NumOfBedrooms { get; set; }
        public int NumOfBathrooms { get; set; }
        public string Address { get; set; }

        public string PropertyTypeName { get; set; } // From PropertyType
        public string HostName { get; set; } // From Host (or User)

        public List<string> AmenityNames { get; set; } = new(); // From many-to-many table
    }

}
