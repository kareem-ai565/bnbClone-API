namespace bnbClone_API.DTOs.ViolationsDTOs
{
    public class ReportedHostDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string LivesIn { get; set; }
        public bool IsVerified { get; set; }
        public decimal Rating { get; set; }
        public string? StripeAccountId { get; set; }
    }
}
