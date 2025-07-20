namespace bnbClone_API.DTOs
{
    public class CreatePropertyImageDto
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPrimary { get; set; } = false;
        public string? Category { get; set; }
    }
}
