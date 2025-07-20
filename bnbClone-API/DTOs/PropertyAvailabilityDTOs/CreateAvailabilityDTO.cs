namespace bnbClone_API.DTOs.PropertyAvailabilityDTOs
{
    public class CreateAvailabilityDTO
    {
        public int PropertyId { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string? BlockedReason { get; set; }
        public decimal Price { get; set; }
        public int MinNights { get; set; } = 1;
    }

}
