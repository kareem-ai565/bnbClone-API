namespace bnbClone_API.DTOs.NotificationsDTOs
{
    public class NotificationResponseDTO
    {
        public int id {  get; set; }
        public int userId { get; set; }
        public string? userName { get; set; }
        public string message { get; set; }
        public bool IsRead { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
    }
}
