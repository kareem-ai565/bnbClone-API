//using bnbClone_API.DTOs;
//using bnbClone_API.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace bnbClone_API.Services.Interfaces
//{
//    public interface IhostVerificationService
//    {
//        Task<IEnumerable<HostVerification>> GetAllHostVerification();
//        Task<HostVerification> AddHostVerification([FromForm] HostVerificationDTO hostVerification);
//        Task<HostVerification> EditHostVerification(int id, [FromForm] HostVerificationDTO hostVerification);
//        Task<HostVerification> GetHostVerificationById(int id);
//        Task<IEnumerable<HostVerification>> GetHostVerificationsByStatus(string status);
//        Task<IEnumerable<HostVerification>> GetHostVerificationsByHostId(int hostId);

// // New methods for status updates
//        Task<HostVerification> ApproveHostVerification(int id, string adminNotes = null);
//        Task<HostVerification> RejectHostVerification(int id, string adminNotes = null);
//    }
//}
using bnbClone_API.DTOs;
using bnbClone_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Services.Interfaces
{
    public interface IhostVerificationService
    {
        // Original methods
        Task<IEnumerable<HostVerification>> GetAllHostVerification();
        Task<HostVerification> AddHostVerification([FromForm] HostVerificationDTO hostVerification);
        Task<HostVerification> EditHostVerification(int id, [FromForm] HostVerificationDTO hostVerification);
        Task<HostVerification> GetHostVerificationById(int id);
        Task<IEnumerable<HostVerification>> GetHostVerificationsByStatus(string status);
        Task<IEnumerable<HostVerification>> GetHostVerificationsByHostId(int hostId);

        // Enhanced methods that include document URLs
        Task<IEnumerable<AdminHostVerificationResponseDto>> GetAllHostVerificationWithDocuments();
        Task<AdminHostVerificationResponseDto> GetHostVerificationByIdWithDocuments(int id);
        Task<IEnumerable<AdminHostVerificationResponseDto>> GetHostVerificationsByStatusWithDocuments(string status);
        Task<IEnumerable<AdminHostVerificationResponseDto>> GetHostVerificationsByHostIdWithDocuments(int hostId);

        // Method to serve individual document files
        Task<(byte[] fileBytes, string contentType, string fileName)> GetDocumentFile(int verificationId, int documentNumber);

        // Status update methods
        Task<HostVerification> ApproveHostVerification(int id, string adminNotes = null);
        Task<HostVerification> RejectHostVerification(int id, string adminNotes = null);
    }
}
