using bnbClone_API.DTOs.Admin;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync()
        {
            // Get total counts
            var totalUsers = await _unitOfWork.Users.CountAsync();
            var activeUsers = await _unitOfWork.Users.CountAsync(u => u.AccountStatus == "active");

            var totalProperties = await _unitOfWork.Properties.CountAsync();
            var activeProperties = await _unitOfWork.Properties.CountAsync(p => p.Status == PropertyStatus.Active.ToString());
            var pendingProperties = await _unitOfWork.Properties.CountAsync(p => p.Status == PropertyStatus.Pending.ToString());

            var pendingViolations = await _unitOfWork.Violations.CountAsync(v => v.Status == ViolationStatus.Pending.ToString());
            var pendingVerifications = await _unitOfWork.HostVerifications.CountAsync(hv => hv.Status == "pending");

            // Get monthly stats for the last 12 months
            var monthlyStats = await GetMonthlyStatsAsync(12);

            return new AdminDashboardSummaryDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                TotalProperties = totalProperties,
                ActiveProperties = activeProperties,
                PendingProperties = pendingProperties,
                TotalBookings = 0, // You'll need to implement booking repository
                PendingViolations = pendingViolations,
                PendingVerifications = pendingVerifications,
                TotalEarnings = 0, // Calculate from bookings
                MonthlyEarnings = 0, // Calculate current month earnings
                MonthlyStats = monthlyStats
            };
        }

        public async Task<List<MonthlyStatsDto>> GetMonthlyStatsAsync(int months = 12)
        {
            var monthlyStats = new List<MonthlyStatsDto>();
            var currentDate = DateTime.UtcNow;

            for (int i = 0; i < months; i++)
            {
                var targetDate = currentDate.AddMonths(-i);
                var startOfMonth = new DateTime(targetDate.Year, targetDate.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1);

                var newUsers = await _unitOfWork.Users.CountAsync(u =>
                    u.CreatedAt >= startOfMonth && u.CreatedAt < endOfMonth);

                var newProperties = await _unitOfWork.Properties.CountAsync(p =>
                    p.CreatedAt >= startOfMonth && p.CreatedAt < endOfMonth);

                monthlyStats.Add(new MonthlyStatsDto
                {
                    Month = targetDate.Month,
                    Year = targetDate.Year,
                    NewUsers = newUsers,
                    NewProperties = newProperties,
                    NewBookings = 0, // Implement when you have booking repository
                    Revenue = 0 // Calculate from bookings
                });
            }

            return monthlyStats.OrderBy(ms => ms.Year).ThenBy(ms => ms.Month).ToList();
        }
    }
}