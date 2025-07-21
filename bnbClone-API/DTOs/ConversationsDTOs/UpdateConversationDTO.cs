using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.ConversationsDTOs
{
    public class UpdateConversationDTO
    {
        [Required]
        public int ConversationId { get; set; }

        [MaxLength(100)]
        public string? Subject { get; set; }

        public int? PropertyId { get; set; }
    }
}
