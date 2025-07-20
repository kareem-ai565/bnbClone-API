namespace bnbClone_API.DTOs.ViolationsDTOs
{
    public class ReportedPropertyDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string? PrimaryImage { get; set; }
    }
}
