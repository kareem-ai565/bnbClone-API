using bnbClone_API.DTOs.Admin;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAdminPropertyService
    {
        Task<IEnumerable<AdminPropertyListDto>> GetAllPropertiesAsync();
        Task<AdminPropertyResponseDto> GetPropertyByIdAsync(int propertyId);
        Task<bool> UpdatePropertyStatusAsync(int propertyId, PropertyStatusUpdateDto request);
        Task<bool> DeletePropertyAsync(int propertyId);
        Task<IEnumerable<AdminPropertyListDto>> GetPropertiesByStatusAsync(string status);
    }
}