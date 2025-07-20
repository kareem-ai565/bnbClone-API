using bnbClone_API.DTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface ICancellationPolicyService
    {
        Task<IEnumerable<CancellationPolicyDto>> GetAllAsync();
        Task<CancellationPolicyDto?> GetByIdAsync(int id);
        Task<CancellationPolicyDto> CreateAsync(CancellationPolicyCreateDto dto);
        Task<bool> UpdateAsync(int id, CancellationPolicyUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
