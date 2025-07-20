namespace bnbClone_API.DTOs
{
    public class BookingPaymentDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PropertyTitle { get; set; }
    }
}
