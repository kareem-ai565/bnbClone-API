using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using bnbClone_API.Models;

namespace bnbClone_API.Models
{
    public class UserUsedPromotion
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public decimal DiscountedAmount { get; set; }
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Promotion Promotion { get; set; } = null!;
        public virtual Booking Booking { get; set; } = null!;
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
