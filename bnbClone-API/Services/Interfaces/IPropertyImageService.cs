using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.DTOs.PropertyDtos;

namespace bnbClone_API.Services.Interfaces
{
    public interface IPropertyImageService
    {
        Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyIdAsync(int propertyId);
        Task<PropertyImageDto> AddImageAsync(int propertyId, CreatePropertyImageDto dto);
        Task<bool> DeleteImageAsync(int imageId);
    }
}
