using bnbClone_API.DTOs.NotificationsDTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;
        public NotificationController(INotificationService notificationService) 
        {
            this.notificationService = notificationService;
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
           var notifications= await notificationService.GetUserNotificationsAsync(userId);
            return Ok(notifications);
        }
        [HttpGet("user/{userId}/count-unread")]
        public async Task<IActionResult> GetUnreadCount(int userId) 
        {
            var count = await notificationService.GetUnreadCountAsync(userId); 
            return Ok(count);
        }
        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id) {
            var success = await notificationService.MarkAsReadAsync(id);
            if(!success) return NotFound("Notification Not Found");
            return Ok("Marked As Read");
        }
        [HttpPut("user/{userId}/mark-all-read")]
        public async Task<IActionResult> MarkAllAsRead(int userId) 
        {
            var success = await notificationService.MarkAllAsReadAsync(userId);
            if (!success) return NotFound("No notifications found to mark as read");
            return Ok("All marked as read");

        }
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO dto)
        {
            var notification = await notificationService.CreateNotificationAsync(dto);
            if (notification == null) return BadRequest("Failed to send Notification");
            return Ok(notification);
        }
        //[HttpPost("broadcast")]
        //public async Task<IActionResult> Broadcast([FromBody] BroadcastNotificationDto dto)
        //{
        //    var notifications = await notificationService.BroadcastNotificationAsync(dto);
        //    return Ok(new
        //    {
        //        Message = $"Broadcast sent to {notifications.Count} users",
        //        SentNotifications = notifications
        //    });
        //}

    }
}
