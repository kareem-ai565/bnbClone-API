namespace bnbClone_API.Models
{
    public class PropertyAvailabilityView
    {
        public int PropertyId { get; set; }
        public DateTime AvailableDate { get; set; }
        public bool IsAvailable { get; set; }
    }
}
