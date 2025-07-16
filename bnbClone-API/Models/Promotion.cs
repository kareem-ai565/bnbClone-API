using bnbClone_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bnbClone_API.Models
{
    public class Promotion
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(20)]
        public string DiscountType { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUses { get; set; }
        public int UsedCount { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<UserUsedPromotion> UserUsedPromotions { get; set; } = new List<UserUsedPromotion>();
    }

    public enum DiscountType
    {
        Percentage,
        FixedAmount
    }
}
