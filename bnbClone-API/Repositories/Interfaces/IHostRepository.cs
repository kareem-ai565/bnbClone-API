using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IHostRepository : IGenericRepository<Models.Host>
    {
        Task<Models.Host> GetByUserIdAsync(int userId);
        Task<Models.Host> GetHostWithDetailsAsync(int hostId);
        Task<IEnumerable<Models.Host>> GetTopRatedHostsAsync(int count = 10);
        Task<bool> IsUserAlreadyHostAsync(int userId);
        Task<Models.Host> GetHostByUserIdAsync(int userId);
        Task<Models.Host> GetHostWithUserAsync(int hostId);
        Task<Models.Host> GetHostWithPropertiesAsync(int hostId);

        Task<Models.Host?> GetHostWithUserByIdAsync(int id);
    }
}