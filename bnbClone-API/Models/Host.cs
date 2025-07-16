using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore.Metadata;


namespace bnbClone_API.Models
{
    public class Host
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public string? AboutMe { get; set; }
        public string? Work { get; set; }
        public decimal Rating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public string? Education { get; set; }
        public string? Languages { get; set; }
        public bool IsVerified { get; set; } = false;
        public decimal TotalEarnings { get; set; } = 0;
        public decimal AvailableBalance { get; set; } = 0;
        public string? StripeAccountId { get; set; }
        public string? DefaultPayoutMethod { get; set; }
        public string? PayoutAccountDetails { get; set; }

        public string? LivesIn { get; set; }
        public string? DreamDestination { get; set; }
        public string? FunFact { get; set; }
        public string? Pets { get; set; }
        public string? ObsessedWith { get; set; }
        public string? SpecialAbout { get; set; }

        // Navigation
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
        public virtual ICollection<HostVerification> Verifications { get; set; } = new List<HostVerification>();
        public virtual ICollection<HostPayout> Payouts { get; set; } = new List<HostPayout>();
    }

}
