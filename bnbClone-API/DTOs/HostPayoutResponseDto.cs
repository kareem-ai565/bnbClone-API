namespace bnbClone_API.DTOs
{
    public class HostPayoutResponseDto
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string PayoutMethod { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string Notes { get; set; }
        public string HostFullName { get; set; }
       
    }
}
