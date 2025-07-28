using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using bnbClone_API.Models;  

namespace bnbClone_API.Models
{
    public class BookingPayment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethodType { get; set; }
        public string Status { get; set; }
        public string TransactionId { get; set; }
        public string PaymentIntentId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public decimal RefundedAmount { get; set; } = 0;
        public string PayementGateWayResponse { get; set; } = string.Empty;

        // Navigation Property
        public virtual Booking Booking { get; set; }
    }
}
