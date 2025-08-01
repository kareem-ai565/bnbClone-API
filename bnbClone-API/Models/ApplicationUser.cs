﻿using Microsoft.AspNetCore.Identity;
using Stripe;
using System.Text.Json.Serialization;


namespace bnbClone_API.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }
        [Url]
        public string? ProfilePictureUrl { get; set; }
        public string? AccountStatus { get; set; } = "active";
        public bool EmailVerified { get; set; } = false;
        public bool PhoneVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; } = "guest";
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [NotMapped]
        public int HostId { get; set; }

        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    }

    public enum AccountStatus
    {
        Active,
        Inactive,
        Suspended,
        Deleted
    }
    public enum UserRole
    {
        Guest,
        Host,
        Admin
    }

    public class UserRoleConstants
    {
        public const string Guest = "Guest";
        public const string Host = "Host";
        public const string Admin = "Admin";
    }

}
