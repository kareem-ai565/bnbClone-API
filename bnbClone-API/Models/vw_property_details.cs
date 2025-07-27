namespace bnbClone_API.Models
{
    public class PropertyDetailsView
    {
        public int PropertyId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string City { get; set; } = null!;
        public decimal PricePerNight { get; set; }
        public decimal CleaningFee { get; set; }
        public decimal ServiceFee { get; set; }
        public string PropertyType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int HostId { get; set; }
        public string HostName { get; set; } = null!;
        public string? HostPicture { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }

}
