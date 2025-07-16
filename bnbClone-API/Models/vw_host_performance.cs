namespace bnbClone_API.Models
{
    public class HostPerformanceView
    {
        public int HostId { get; set; }
        public string HostName { get; set; } = null!;
        public int TotalProperties { get; set; }
        public int TotalBookings { get; set; }
        public double? AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
