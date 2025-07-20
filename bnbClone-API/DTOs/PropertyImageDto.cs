namespace bnbClone_API.DTOs
{
    public class PropertyImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPrimary { get; set; }
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
