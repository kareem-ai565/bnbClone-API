using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces.admin
{
    public interface IHostVerificationRepository : IGenericRepository<HostVerification>
    {
        Task<IEnumerable<HostVerification>> GetAllWithHostAsync();
        Task<HostVerification> GetByIdWithHostAsync(int id);
        Task<IEnumerable<HostVerification>> GetByStatusAsync(string status);
        Task UpdateStatusAsync(int id, string status, string adminNotes = null);
    }
}
