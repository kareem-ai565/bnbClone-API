namespace bnbClone_API.Models
{
    public enum ViolationType
    {
        PropertyMisrepresentation,
        HostMisconduct,
        SafetyIssue,
        PolicyViolation,
        FraudulentActivity,
        Other
    }

    public enum ViolationStatus
    {
        Pending,
        UnderReview,
        Resolved,
        Dismissed
    }

    public class Violation
    {
        public int Id { get; set; }
        public int ReportedById { get; set; } // User who reported the violation
        public int? ReportedPropertyId { get; set; } // Property involved (if applicable)
        public int? ReportedHostId { get; set; } // Host involved (if applicable)
        public string ViolationType { get; set; } = Models.ViolationType.Other.ToString();
        public string Description { get; set; }
        public string Status { get; set; } = ViolationStatus.Pending.ToString();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public string? AdminNotes { get; set; }
        public DateTime? ResolvedAt { get; set; }

        // Navigation Properties
        public virtual ApplicationUser ReportedBy { get; set; }
        public virtual Property ReportedProperty { get; set; }
        public virtual Host ReportedHost { get; set; }
    }
}
