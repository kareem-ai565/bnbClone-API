namespace bnbClone_API.DTOs
{
    public class PaymentCreateDto
    {
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethodType { get; set; } = "card";
    }
}
