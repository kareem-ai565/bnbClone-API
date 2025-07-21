using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.ConversationsDTOs
{
    public class ConversationResponseDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int? User1Id { get; set; }
        public string? User1Name { get; set; }
        [Required]
        public int? User2Id { get; set; }
        public string? User2Name { get; set; }

        [Required]
        public int? PropertyId { get; set; }
        public string? PropertyName { get; set; }

        [MaxLength(100)]
        public string? Subject { get; set; }
    }
}
