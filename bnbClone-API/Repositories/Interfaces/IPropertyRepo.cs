using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IPropertyRepo : IGenericRepo<Property>
    {
        // Add Property-specific methods here if needed in future
        Task<List<Property>> SearchAsync(PropertySearchDto dto);
        Task<Property?> GetDetailsByIdAsync(int id);
        Task<IEnumerable<Property>> FindByHostIdAsync(int hostId);


    }
}
