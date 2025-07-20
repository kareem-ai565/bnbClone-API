namespace bnbClone_API.DTOs
{
    public class CreatePropertyDto
    {
        public int HostId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PropertyType { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal CleaningFee { get; set; }
        public decimal ServiceFee { get; set; }
        public int MinNights { get; set; }
        public int MaxNights { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int MaxGuests { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public bool InstantBook { get; set; } = false;
        public int? CancellationPolicyId { get; set; }
    }

}
