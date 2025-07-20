namespace bnbClone_API.DTOs
{
    public class PaymentResponseDto
    {
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
