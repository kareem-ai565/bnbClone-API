namespace bnbClone_API.DTOs.ViolationsDTOs
{
    public class ViolationListDTO
    {
        public int Id { get; set; }
        public string ViolationType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public string ReporterName { get; set; }
        public string? PropertyTitle { get; set; }
        public string? HostName { get; set; }
    }

}
