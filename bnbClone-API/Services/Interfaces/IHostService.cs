using bnbClone_API.DTOs.HostDTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IHostService
    {
        Task<IEnumerable<HostDto>> GetAllHostsAsync();            // api/hosts
        Task<HostDto?> GetHostByIdAsync(int hostId);              // api/hosts/{id}
        Task<HostDto?> GetHostByUserIdAsync(int userId);          // api/hosts/me
        Task<bool> UpdateHostByUserIdAsync(int userId, HostUpdateDto dto); // PUT /api/hosts/me
    }
}

