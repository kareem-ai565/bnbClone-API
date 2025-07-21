namespace bnbClone_API.DTOs.MessagesDTOs
{
    public class MessageDTO
    {
       
            public int Id { get; set; }
            public int ConversationId { get; set; }
            public int SenderId { get; set; }
            public int ReceiverId { get; set; }
            public string Content { get; set; }
            public DateTime SentAt { get; set; } = DateTime.Now;
            public DateTime? ReadAt { get; set; }
        
    }
}
