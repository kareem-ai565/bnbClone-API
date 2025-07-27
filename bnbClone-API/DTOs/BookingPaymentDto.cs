using bnbClone_API.Models;

namespace bnbClone_API.DTOs
{
    public class BookingPaymentDto
    {
       

        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // pending, confirmed
        public string TransactionId { get; set; } // Checkout Session ID
        public string PaymentIntentId { get; set; } // Stripe PaymentIntent ID
        public string PaymentMethodType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string PaymentGatewayResponse { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public decimal RefundedAmount { get; set; } = 0;
        public string PayementGateWayResponse { get; set; } = string.Empty;
        public virtual Booking Booking { get; set; }
       

    }
}
