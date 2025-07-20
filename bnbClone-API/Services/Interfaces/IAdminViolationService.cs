using bnbClone_API.DTOs.Admin;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAdminViolationService
    {
        Task<IEnumerable<AdminViolationListDto>> GetAllViolationsAsync();
        Task<AdminViolationResponseDto> GetViolationByIdAsync(int violationId);
        Task<bool> UpdateViolationStatusAsync(int violationId, ViolationStatusUpdateDto request);
        Task<IEnumerable<AdminViolationListDto>> GetViolationsByStatusAsync(string status);
    }
}