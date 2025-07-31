using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Services.Impelementations
{
    public class hostVerificationService : IhostVerificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor _httpContext;

        public hostVerificationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContext)
        {
            this.unitOfWork = unitOfWork;
            this._httpContext = httpContext;
        }

        public async Task<IEnumerable<HostVerification>> GetAllHostVerification()
        {
            IEnumerable<HostVerification> hosts = await unitOfWork.hostVerification.GetAllAsync();
            return hosts;
        }

        //added by kareemx
        public async Task<HostVerification> GetHostVerificationById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid verification ID.");
            }

            var verification = await unitOfWork.hostVerification.GetByIdAsync(id);
            if (verification == null)
            {
                throw new KeyNotFoundException($"Host verification with ID {id} not found.");
            }

            return verification;
        }

        public async Task<IEnumerable<HostVerification>> GetHostVerificationsByStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Status cannot be empty.");
            }

            var allVerifications = await unitOfWork.hostVerification.GetAllAsync();
            var filteredVerifications = allVerifications.Where(v =>
                v.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            return filteredVerifications;
        }

        public async Task<IEnumerable<HostVerification>> GetHostVerificationsByHostId(int hostId)
        {
            if (hostId <= 0)
            {
                throw new ArgumentException("Invalid host ID.");
            }

            var allVerifications = await unitOfWork.hostVerification.GetAllAsync();
            var hostVerifications = allVerifications.Where(v => v.HostId == hostId);

            return hostVerifications;
        }

        public async Task<HostVerification> AddHostVerification([FromForm] HostVerificationDTO hostVerificationDto)
        {
            // Validate input
            if (hostVerificationDto?.DocumentUrl1 == null || hostVerificationDto?.DocumentUrl2 == null)
            {
                throw new ArgumentException("Both document files are required.");
            }

            var hostID = _httpContext.HttpContext.User.FindFirst("HostId")?.Value;
            if (hostID == null)
            {
                throw new UnauthorizedAccessException("Host ID not found in the token.");
            }

            var hostIdInt = int.Parse(hostID);
            var verificationType = hostVerificationDto.Type.ToString();

            // Check if this host already has a verification of this type
            var existingVerifications = await unitOfWork.hostVerification.GetAllAsync();
            var existingVerification = existingVerifications.FirstOrDefault(v =>
                v.HostId == hostIdInt && v.Type == verificationType);

            if (existingVerification != null)
            {
                throw new InvalidOperationException($"Host already has a {verificationType} verification. Use PUT to update existing verification.");
            }

            HostVerification host = new HostVerification
            {
                HostId = hostIdInt,
                Type = verificationType,
                Status = VerificationStatus.pending.ToString(),
                SubmittedAt = hostVerificationDto.SubmittedAt,
            };

            // Ensure directory exists
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName1 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerificationDto.DocumentUrl1.FileName)}";
            var fileName2 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerificationDto.DocumentUrl2.FileName)}";

            var filePath1 = Path.Combine(folderPath, fileName1);
            var filePath2 = Path.Combine(folderPath, fileName2);

            // Use async file operations
            using (var fileStream1 = new FileStream(filePath1, FileMode.CreateNew))
            {
                await hostVerificationDto.DocumentUrl1.CopyToAsync(fileStream1);
            }

            using (var fileStream2 = new FileStream(filePath2, FileMode.CreateNew))
            {
                await hostVerificationDto.DocumentUrl2.CopyToAsync(fileStream2);
            }

            host.DocumentUrl1 = fileName1;
            host.DocumentUrl2 = fileName2;

            await unitOfWork.hostVerification.AddAsync(host);
            await unitOfWork.SaveAsync();

            return host;
        }

        public async Task<HostVerification> EditHostVerification(int id, [FromForm] HostVerificationDTO hostVerification)
        {
            // Validate input
            if (hostVerification?.DocumentUrl1 == null || hostVerification?.DocumentUrl2 == null)
            {
                throw new ArgumentException("Both document files are required.");
            }

            HostVerification host = await unitOfWork.hostVerification.GetByIdAsync(id);
            if (host == null)
            {
                throw new ArgumentException($"Host verification with ID {id} not found.");
            }

            host.Type = hostVerification.Type.ToString();
            host.SubmittedAt = hostVerification.SubmittedAt;
            // Reset status to Pending when updating
            host.Status = VerificationStatus.pending.ToString();

            // Delete old files if they exist
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

            if (!string.IsNullOrEmpty(host.DocumentUrl1))
            {
                var oldFilePath1 = Path.Combine(folderPath, host.DocumentUrl1);
                if (File.Exists(oldFilePath1))
                {
                    File.Delete(oldFilePath1);
                }
            }

            if (!string.IsNullOrEmpty(host.DocumentUrl2))
            {
                var oldFilePath2 = Path.Combine(folderPath, host.DocumentUrl2);
                if (File.Exists(oldFilePath2))
                {
                    File.Delete(oldFilePath2);
                }
            }

            // Ensure directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName1 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerification.DocumentUrl1.FileName)}";
            var fileName2 = $"{Guid.NewGuid()}{Path.GetExtension(hostVerification.DocumentUrl2.FileName)}";

            var filePath1 = Path.Combine(folderPath, fileName1);
            var filePath2 = Path.Combine(folderPath, fileName2);

            // Use async file operations
            using (var fileStream1 = new FileStream(filePath1, FileMode.CreateNew))
            {
                await hostVerification.DocumentUrl1.CopyToAsync(fileStream1);
            }

            using (var fileStream2 = new FileStream(filePath2, FileMode.CreateNew))
            {
                await hostVerification.DocumentUrl2.CopyToAsync(fileStream2);
            }

            host.DocumentUrl1 = fileName1;
            host.DocumentUrl2 = fileName2;

            await unitOfWork.SaveAsync();

            return host;
        }

         // New method to approve host verification
        public async Task<HostVerification> ApproveHostVerification(int id, string adminNotes = null)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid verification ID.");
            }

            var verification = await unitOfWork.hostVerification.GetByIdAsync(id);
            if (verification == null)
            {
                throw new KeyNotFoundException($"Host verification with ID {id} not found.");
            }

            // Check if already approved
            if (verification.Status.Equals("approved", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Host verification is already approved.");
            }

            verification.Status = VerificationStatus.approved.ToString();
            verification.VerifiedAt = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(adminNotes))
            {
                verification.AdminNotes = adminNotes;
            }

            await unitOfWork.SaveAsync();
            return verification;
        }

        // New method to reject host verification
        public async Task<HostVerification> RejectHostVerification(int id, string adminNotes = null)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid verification ID.");
            }

            var verification = await unitOfWork.hostVerification.GetByIdAsync(id);
            if (verification == null)
            {
                throw new KeyNotFoundException($"Host verification with ID {id} not found.");
            }

            // Check if already rejected
            if (verification.Status.Equals("rejected", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("Host verification is already rejected.");
            }

            verification.Status = VerificationStatus.rejected.ToString();
            verification.VerifiedAt = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(adminNotes))
            {
                verification.AdminNotes = adminNotes;
            }

            await unitOfWork.SaveAsync();
            return verification;
        }
    }
}