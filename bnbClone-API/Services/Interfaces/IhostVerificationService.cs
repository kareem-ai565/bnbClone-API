using bnbClone_API.DTOs;
using bnbClone_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Services.Interfaces
{
    public interface IhostVerificationService
    {
        Task<IEnumerable<HostVerification>> GetAllHostVerification();
        Task<HostVerification> AddHostVerification([FromForm] HostVerificationDTO hostVerification);
        Task<HostVerification> EditHostVerification(int id, [FromForm] HostVerificationDTO hostVerification);

    }
}
