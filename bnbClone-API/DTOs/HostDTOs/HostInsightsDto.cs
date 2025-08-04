namespace bnbClone_API.DTOs.HostDTOs
{
    public class HostInsightsDto
    {
        public decimal TotalCompletedPayouts { get; set; }
        public decimal TotalProcessingPayouts { get; set; }
        public int PropertyCount { get; set; }

        public decimal AvailableBalance { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalProcessing { get; set; }
        public decimal TotalCompleted { get; set; }
        public int HostId { get; set; }

    }
}
