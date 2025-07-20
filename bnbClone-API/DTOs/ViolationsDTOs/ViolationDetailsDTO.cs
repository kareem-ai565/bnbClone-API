namespace bnbClone_API.DTOs.ViolationsDTOs
{
    public class ViolationDetailsDTO
    {
        public int Id { get; set; }
        public string ViolationType { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? AdminNotes { get; set; }

        public ReportedUserDTO Reporter { get; set; }
        public ReportedHostDTO? Host { get; set; }
        public ReportedPropertyDTO? Property { get; set; }
    }

}
