using AutoMapper;
using bnbClone_API.DTOs.NotificationsDTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Http.HttpResults;

namespace bnbClone_API.Services.Impelementations
{
    public class NotificationService : INotificationService
    {
       
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public NotificationService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public Task<List<NotificationResponseDTO>> BroadcastNotificationAsync(BroadcastNotificationDTO dto)
        {
            throw new NotImplementedException();
            //implement Later
        }

        public async Task<NotificationResponseDTO?> CreateNotificationAsync(CreateNotificationDTO dto)
        {
           var notification = mapper.Map<Notification>(dto);
            notification.CreatedAt = DateTime.UtcNow;
            notification.IsRead = false;

            await unitOfWork.NotificationRepo.AddAsync(notification);
            await unitOfWork.SaveAsync();
            return mapper.Map<NotificationResponseDTO>(notification);

        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await unitOfWork.NotificationRepo.GetUnreadCountAsync(userId);
        }

        public async Task<List<NotificationResponseDTO>> GetUserNotificationsAsync(int userId)
        {
            var notifications = await unitOfWork.NotificationRepo.GetForUserAsync(userId);
            return mapper.Map<List<NotificationResponseDTO>>(notifications);

        }

        public async Task<bool> MarkAllAsReadAsync(int userId)
        {
            var marked = await unitOfWork.NotificationRepo.MarkAllAsReadAsync(userId);
            if(!marked) return false;
            var saved = await unitOfWork.SaveAsync();
            return saved > 0;
        }

        public async Task<bool> MarkAsReadAsync(int id)
        {
            var marked = await unitOfWork.NotificationRepo.MarkAsReadAsync(id);
            if (!marked) return false;
            var saved = await unitOfWork.SaveAsync();
            return saved > 0;
        }
    }
}
