using bnbClone_API.DTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IHostPayoutService
    {
        Task<IEnumerable<HostPayoutResponseDto>> GetAllAsync();
        Task<IEnumerable<HostPayoutResponseDto>> GetByHostIdAsync(int hostId);
        Task<HostPayoutResponseDto> GetByIdAsync(int id);
    }
}
