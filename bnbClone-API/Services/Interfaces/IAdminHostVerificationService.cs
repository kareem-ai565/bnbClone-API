using bnbClone_API.DTOs.Admin;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAdminHostVerificationService
    {
        Task<IEnumerable<AdminHostVerificationListDto>> GetAllVerificationsAsync();
        Task<AdminHostVerificationResponseDto> GetVerificationByIdAsync(int verificationId);
        Task<bool> UpdateVerificationStatusAsync(int verificationId, HostVerificationStatusUpdateDto request);
        Task<IEnumerable<AdminHostVerificationListDto>> GetVerificationsByStatusAsync(string status);
    }
}