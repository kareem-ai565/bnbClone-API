using bnbClone_API.DTOs.Admin;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class AdminViolationService : IAdminViolationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminViolationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AdminViolationListDto>> GetAllViolationsAsync()
        {
            var violations = await _unitOfWork.Violations.GetAllWithDetailsAsync();
            return violations.Select(v => new AdminViolationListDto
            {
                Id = v.Id,
                ReporterName = $"{v.ReportedBy.FirstName} {v.ReportedBy.LastName}",
                ViolationType = v.ViolationType,
                Status = v.Status,
                CreatedAt = v.CreatedAt,
                PropertyTitle = v.ReportedProperty?.Title,
                ReportedHostName = v.ReportedHost != null ?
                    $"{v.ReportedHost.User.FirstName} {v.ReportedHost.User.LastName}" : null
            });
        }

        public async Task<AdminViolationResponseDto> GetViolationByIdAsync(int violationId)
        {
            var violation = await _unitOfWork.Violations.GetByIdWithDetailsAsync(violationId);
            if (violation == null) return null;

            return new AdminViolationResponseDto
            {
                Id = violation.Id,
                ReportedById = violation.ReportedById,
                ReporterName = $"{violation.ReportedBy.FirstName} {violation.ReportedBy.LastName}",
                ReporterEmail = violation.ReportedBy.Email,
                ReportedPropertyId = violation.ReportedPropertyId,
                PropertyTitle = violation.ReportedProperty?.Title,
                ReportedHostId = violation.ReportedHostId,
                ReportedHostName = violation.ReportedHost != null ?
                    $"{violation.ReportedHost.User.FirstName} {violation.ReportedHost.User.LastName}" : null,
                ViolationType = violation.ViolationType,
                Description = violation.Description,
                Status = violation.Status,
                CreatedAt = violation.CreatedAt,
                UpdatedAt = violation.UpdatedAt,
                AdminNotes = violation.AdminNotes,
                ResolvedAt = violation.ResolvedAt
            };
        }

        public async Task<bool> UpdateViolationStatusAsync(int violationId, ViolationStatusUpdateDto request)
        {
            await _unitOfWork.Violations.UpdateStatusAsync(violationId, request.Status, request.AdminNotes);
            var result = await _unitOfWork.SaveAsync();
            return result > 0;
        }

        public async Task<IEnumerable<AdminViolationListDto>> GetViolationsByStatusAsync(string status)
        {
            var violations = await _unitOfWork.Violations.GetByStatusAsync(status);
            return violations.Select(v => new AdminViolationListDto
            {
                Id = v.Id,
                ReporterName = $"{v.ReportedBy.FirstName} {v.ReportedBy.LastName}",
                ViolationType = v.ViolationType,
                Status = v.Status,
                CreatedAt = v.CreatedAt,
                PropertyTitle = v.ReportedProperty?.Title,
                ReportedHostName = v.ReportedHost != null ?
                    $"{v.ReportedHost.User.FirstName} {v.ReportedHost.User.LastName}" : null
            });
        }
    }
}