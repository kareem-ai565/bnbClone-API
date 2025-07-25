using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bnbClone_API.Models
{
    public enum PropertyStatus
    {
        Active,
        Pending,
        Suspended,
    }

    public class Property
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int HostId { get; set; }
        public int? CategoryId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        [StringLength(50)]
        public string PropertyType { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        [Column(TypeName = "decimal(10,8)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal Longitude { get; set; }

        public string Currency { get; set; } = "USD";

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CleaningFee { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal ServiceFee { get; set; }

        public int MinNights { get; set; } = 1;
        public int MaxNights { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int MaxGuests { get; set; }

        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }

        public bool InstantBook { get; set; } = false;

        public string Status { get; set; } = PropertyStatus.Pending.ToString();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public int? CancellationPolicyId { get; set; }

        // Navigation Properties
        [ForeignKey("HostId")]
        public virtual Host Host { get; set; } = null!;

        [ForeignKey("CancellationPolicyId")]
        public virtual CancellationPolicy CancellationPolicy { get; set; }

        [ForeignKey("CategoryId")]
        public virtual PropertyCategory Category { get; set; }

        public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public virtual ICollection<Favourite> Favourites { get; set; } = new List<Favourite>();
        public virtual ICollection<PropertyAvailability> Availabilities { get; set; } = new List<PropertyAvailability>();
        public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

        // New navigation property for the join table
        public virtual ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();


        //////new added 
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
