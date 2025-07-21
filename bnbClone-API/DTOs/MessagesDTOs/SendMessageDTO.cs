using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.MessagesDTOs
{
    public class SendMessageDTO
    {
        
            [Required]
            public int ConversationId { get; set; }

            [Required]
            public int SenderId { get; set; }

            [Required]
            public int ReceiverId { get; set; }

            [Required]
            [MaxLength(1000)]
            public string Content { get; set; } = string.Empty;
        
    }
}
