using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace bnbClone_API.Models
{
    public class HostPayout
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } // Pending, Processing, Completed, Failed
        public string PayoutMethod { get; set; } // Bank Transfer, PayPal, etc.
        public string PayoutAccountDetails { get; set; } // Encrypted account details
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }
        public string Notes { get; set; }

        // Navigation Properties
        [JsonIgnore]
        public virtual Host Host { get; set; }
    }
}
