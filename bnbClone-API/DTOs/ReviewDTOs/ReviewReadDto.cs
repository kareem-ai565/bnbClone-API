namespace bnbClone_API.DTOs.ReviewDTOs
{
    public class ReviewReadDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ReviewerName { get; set; } 
        public string PropertyName { get; set; } 
    }
}
