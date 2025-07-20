using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.AdminDTOs
{
    public class AdminUserResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string AccountStatus { get; set; }
        public bool EmailVerified { get; set; }
        public bool PhoneVerified { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }
    }

    public class BanUserRequestDto
    {
        [Required]
        public string Reason { get; set; }
        public DateTime? BanUntil { get; set; }
    }

    public class AdminUserListDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string AccountStatus { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}

// DTOs/Admin/PropertyManagementDtos.cs
namespace bnbClone_API.DTOs.Admin
{
    public class AdminPropertyResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PropertyType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public decimal PricePerNight { get; set; }
        public string Currency { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public AdminHostSummaryDto Host { get; set; }
    }

    public class AdminHostSummaryDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class PropertyStatusUpdateDto
    {
        [Required]
        public string Status { get; set; } // Active, Pending, Suspended
        public string AdminNotes { get; set; }
    }

    public class AdminPropertyListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PropertyType { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal PricePerNight { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string HostName { get; set; }
        public string HostEmail { get; set; }
    }
}

// DTOs/Admin/ViolationManagementDtos.cs
namespace bnbClone_API.DTOs.Admin
{
    public class AdminViolationResponseDto
    {
        public int Id { get; set; }
        public int ReportedById { get; set; }
        public string ReporterName { get; set; }
        public string ReporterEmail { get; set; }
        public int? ReportedPropertyId { get; set; }
        public string PropertyTitle { get; set; }
        public int? ReportedHostId { get; set; }
        public string ReportedHostName { get; set; }
        public string ViolationType { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string AdminNotes { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class ViolationStatusUpdateDto
    {
        [Required]
        public string Status { get; set; } // Pending, UnderReview, Resolved, Dismissed
        public string AdminNotes { get; set; }
    }

    public class AdminViolationListDto
    {
        public int Id { get; set; }
        public string ReporterName { get; set; }
        public string ViolationType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PropertyTitle { get; set; }
        public string ReportedHostName { get; set; }
    }
}

// DTOs/Admin/HostVerificationDtos.cs
namespace bnbClone_API.DTOs.Admin
{
    public class AdminHostVerificationResponseDto
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public string HostName { get; set; }
        public string HostEmail { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string DocumentUrl1 { get; set; }
        public string DocumentUrl2 { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }

    public class HostVerificationStatusUpdateDto
    {
        [Required]
        public string Status { get; set; } // pending, approved, rejected
        public string AdminNotes { get; set; }
    }

    public class AdminHostVerificationListDto
    {
        public int Id { get; set; }
        public string HostName { get; set; }
        public string HostEmail { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}

// DTOs/Admin/NotificationDtos.cs
namespace bnbClone_API.DTOs.Admin
{
    public class AdminNotificationRequestDto
    {
        [Required]
        public string Message { get; set; }
        public List<int> UserIds { get; set; } = new List<int>(); // Empty list means send to all users
        public bool SendToAllUsers { get; set; } = false;
    }

    public class AdminNotificationResponseDto
    {
        public string Message { get; set; }
        public int RecipientCount { get; set; }
        public DateTime SentAt { get; set; }
    }
}

// DTOs/Admin/DashboardDtos.cs
namespace bnbClone_API.DTOs.Admin
{
    public class AdminDashboardSummaryDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalProperties { get; set; }
        public int ActiveProperties { get; set; }
        public int PendingProperties { get; set; }
        public int TotalBookings { get; set; }
        public int PendingViolations { get; set; }
        public int PendingVerifications { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal MonthlyEarnings { get; set; }
        public List<MonthlyStatsDto> MonthlyStats { get; set; } = new List<MonthlyStatsDto>();
    }

    public class MonthlyStatsDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int NewUsers { get; set; }
        public int NewProperties { get; set; }
        public int NewBookings { get; set; }
        public decimal Revenue { get; set; }
    }
}

