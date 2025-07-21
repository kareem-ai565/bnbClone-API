using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.NotificationsDTOs
{
    public class CreateNotificationDTO
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public int? SenderId { get; set; }
        [MaxLength(1000)]
        public string message { get; set; }
    }
}
