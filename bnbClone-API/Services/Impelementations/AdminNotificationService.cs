using bnbClone_API.DTOs.Admin;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class AdminNotificationService : IAdminNotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminNotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminNotificationResponseDto> SendNotificationAsync(AdminNotificationRequestDto request)
        {
            if (request.SendToAllUsers)
            {
                return await SendNotificationToAllUsersAsync(request.Message);
            }
            else
            {
                return await SendNotificationToSpecificUsersAsync(request.Message, request.UserIds);
            }
        }

        public async Task<AdminNotificationResponseDto> SendNotificationToAllUsersAsync(string message)
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var notifications = users.Select(user => new Notification
            {
                UserId = user.Id,
                Message = message,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _unitOfWork.Notifications.BulkInsertAsync(notifications);
            await _unitOfWork.SaveAsync();

            return new AdminNotificationResponseDto
            {
                Message = message,
                RecipientCount = notifications.Count,
                SentAt = DateTime.UtcNow
            };
        }

        public async Task<AdminNotificationResponseDto> SendNotificationToSpecificUsersAsync(string message, List<int> userIds)
        {
            var notifications = userIds.Select(userId => new Notification
            {
                UserId = userId,
                Message = message,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _unitOfWork.Notifications.BulkInsertAsync(notifications);
            var result = await _unitOfWork.SaveAsync();

            return new AdminNotificationResponseDto
            {
                Message = message,
                RecipientCount = notifications.Count,
                SentAt = DateTime.UtcNow
            };
        }
    }
}