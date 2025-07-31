using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;


namespace bnbClone_API.DTOs
{
    public class HostVerificationDTO
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TypeOfVerification Type { get; set; }
        public IFormFile DocumentUrl1 { get; set; }
        public IFormFile DocumentUrl2 { get; set; }
        public DateTime SubmittedAt { get;} = DateTime.Now;

        //[JsonIgnore]
        //public DateTime? VerifiedAt { get; set; }// Make sure this exists
        //public string AdminNotes { get; set; }
    }

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
        public string AdminNotes { get; set; }

        //add by kim
        public string DocumentUrl1Full { get; set; }
        public string DocumentUrl2Full { get; set; }
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
        public DateTime? VerifiedAt { get; set; } // Added VerifiedAt for list view

    }
    // New DTO for bulk operations
    public class BulkStatusUpdateDto
    {
        [Required]
        public List<int> VerificationIds { get; set; }

        [Required]
        [RegularExpression("^(pending|approved|rejected)$", ErrorMessage = "Status must be 'pending', 'approved', or 'rejected'")]
        public string Status { get; set; }

        [MaxLength(1000, ErrorMessage = "Admin notes cannot exceed 1000 characters")]
        public string AdminNotes { get; set; }
    }

    // Response DTO for verification statistics (optional)
    public class VerificationStatsDto
    {
        public int TotalVerifications { get; set; }
        public int PendingVerifications { get; set; }
        public int ApprovedVerifications { get; set; }
        public int RejectedVerifications { get; set; }
        public Dictionary<string, int> VerificationsByType { get; set; }
    }
    public class AdminNotesDto
    {
        [MaxLength(1000, ErrorMessage = "Admin notes cannot exceed 1000 characters")]
        public string AdminNotes { get; set; }
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypeOfVerification
    {
        [Display(Name = "Government ID")]
        government_id,

        [Display(Name = "Driving License")]
        drivinglicense,

        [Display(Name = "Passport")]
        passport,

        [Display(Name = "Other")]
        other
    }

    public enum VerificationStatus
    {
        pending,
        approved,
        rejected
    }
}
