using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface ICancellationPolicyRepo : IGenericRepo<CancellationPolicy>
    {
        Task<IEnumerable<CancellationPolicy>> GetAllAsync();
        Task<CancellationPolicy?> GetByIdAsync(int id);
    }
}
