using bnbClone_API.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace bnbClone_API.Models
{
    public enum BookingStatus
    {
        Confirmed,
        Denied,
        Pending,
        Cancelled,
        Completed
    }

    public class Booking
    {
        public int Id { get; set; }

        public int PropertyId { get; set; }
        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; }


        public int GuestId { get; set; }
        [ForeignKey("GuestId")]
        public virtual ApplicationUser Guest { get; set; }

        public int TotalGuests { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string CheckInStatus { get; set; } = "pending";
        public string CheckOutStatus { get; set; } = "pending";
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int PromotionId { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Review Review { get; set; }
        public virtual UserUsedPromotion UsedPromotion { get; set; }
        public virtual ICollection<BookingPayment> Payments { get; set; } = new List<BookingPayment>();
    }
}
