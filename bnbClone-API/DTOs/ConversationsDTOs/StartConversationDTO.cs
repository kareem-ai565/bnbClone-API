using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.ConversationsDTOs
{
    public class StartConversationDTO
    {
        [Required]
        public int User1Id { get; set; }

        [Required]
        public int User2Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [MaxLength(100)]
        public string Subject { get; set; } = string.Empty;
    }
}
