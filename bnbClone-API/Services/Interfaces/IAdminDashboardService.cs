using bnbClone_API.DTOs.Admin;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync();
        Task<List<MonthlyStatsDto>> GetMonthlyStatsAsync(int months = 12);
    }
}