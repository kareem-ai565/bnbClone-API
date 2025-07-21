using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs.NotificationsDTOs
{
    public class BroadcastNotificationDTO
    {
        public int? senderId {  get; set; }
        [MaxLength(1000)]
        [Required]
        public string message { get; set; }
    }
}
