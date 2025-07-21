using bnbClone_API.DTOs.AdminDTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AdminUserListDto>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            return users.Select(u => new AdminUserListDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                AccountStatus = u.AccountStatus,
                Role = u.Role,
                CreatedAt = u.CreatedAt,
                LastLogin = u.LastLogin
            });
        }

        public async Task<AdminUserResponseDto> GetUserByIdAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return null;

            return new AdminUserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth,
                Gender = user.Gender,
                ProfilePictureUrl = user.ProfilePictureUrl,
                AccountStatus = user.AccountStatus,
                EmailVerified = user.EmailVerified,
                PhoneVerified = user.PhoneVerified,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                LastLogin = user.LastLogin,
                Role = user.Role
            };
        }

        public async Task<bool> BanUserAsync(int userId, BanUserRequestDto request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return false;

            user.AccountStatus = "suspended";
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);

            // Create notification for the banned user
            var notification = new Notification
            {
                UserId = userId,
                Message = $"Your account has been suspended. Reason: {request.Reason}",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Notifications.AddAsync(notification);

            var result = await _unitOfWork.SaveAsync();
            return result > 0;
        }

        public async Task<bool> UnbanUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return false;

            user.AccountStatus = "active";
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);

            var result = await _unitOfWork.SaveAsync();
            return result > 0;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) return false;

            _unitOfWork.Users.Remove(user);

            var result = await _unitOfWork.SaveAsync();
            return result > 0;
        }
    }
}