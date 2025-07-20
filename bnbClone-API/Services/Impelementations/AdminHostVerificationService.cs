using bnbClone_API.DTOs.Admin;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class AdminHostVerificationService : IAdminHostVerificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminHostVerificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AdminHostVerificationListDto>> GetAllVerificationsAsync()
        {
            var verifications = await _unitOfWork.HostVerifications.GetAllWithHostAsync();
            return verifications.Select(hv => new AdminHostVerificationListDto
            {
                Id = hv.Id,
                HostName = $"{hv.Host.User.FirstName} {hv.Host.User.LastName}",
                HostEmail = hv.Host.User.Email,
                Type = hv.Type,
                Status = hv.Status,
                SubmittedAt = hv.SubmittedAt
            });
        }

        public async Task<AdminHostVerificationResponseDto> GetVerificationByIdAsync(int verificationId)
        {
            var verification = await _unitOfWork.HostVerifications.GetByIdWithHostAsync(verificationId);
            if (verification == null) return null;

            return new AdminHostVerificationResponseDto
            {
                Id = verification.Id,
                HostId = verification.HostId,
                HostName = $"{verification.Host.User.FirstName} {verification.Host.User.LastName}",
                HostEmail = verification.Host.User.Email,
                Type = verification.Type,
                Status = verification.Status,
                DocumentUrl1 = verification.DocumentUrl1,
                DocumentUrl2 = verification.DocumentUrl2,
                SubmittedAt = verification.SubmittedAt,
                VerifiedAt = verification.VerifiedAt
            };
        }

        public async Task<bool> UpdateVerificationStatusAsync(int verificationId, HostVerificationStatusUpdateDto request)
        {
            await _unitOfWork.HostVerifications.UpdateStatusAsync(verificationId, request.Status, request.AdminNotes);
            var result = await _unitOfWork.SaveAsync();
            return result > 0;
        }

        public async Task<IEnumerable<AdminHostVerificationListDto>> GetVerificationsByStatusAsync(string status)
        {
            var verifications = await _unitOfWork.HostVerifications.GetByStatusAsync(status);
            return verifications.Select(hv => new AdminHostVerificationListDto
            {
                Id = hv.Id,
                HostName = $"{hv.Host.User.FirstName} {hv.Host.User.LastName}",
                HostEmail = hv.Host.User.Email,
                Type = hv.Type,
                Status = hv.Status,
                SubmittedAt = hv.SubmittedAt
            });
        }
    }
}