namespace bnbClone_API.DTOs.PropertyDtos
{
    public class PropertyAvailabilityDto
    {
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public string BlockedReason { get; set; }
        public int MinNights { get; set; }
    }

}
