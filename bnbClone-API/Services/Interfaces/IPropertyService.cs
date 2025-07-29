using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;

namespace bnbClone_API.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<Property>> GetAllAsync();
        Task<Property?> GetByIdAsync(int id);
        Task<Property> AddAsync(Property property);
        Task<bool> UpdateAsync(Property property);
        Task<bool> DeleteAsync(int id);
        Task<List<Property>> SearchAsync(PropertySearchDto dto);

        Task<bool> UpdateStep5AmenitiesAsync(int propertyId, List<int> amenityIds);
        Task<Property?> GetByIdWithAmenitiesAsync(int id); 


    }
}
