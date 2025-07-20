namespace bnbClone_API.DTOs
{
    public class HostVerificationDTO
    {

      
        public int HostId { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Status { get; set; } = "pending";
        public IFormFile DocumentUrl1 { get; set; }
        public IFormFile DocumentUrl2 { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public DateTime? VerifiedAt { get; set; }
    }
}
