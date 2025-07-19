using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IPropertyAmenityRepo : IGenericRepo<PropertyAmenity>
    {
        Task<PropertyAmenity> DeleteAsync(int propertyId, int AmenityId);
    }
}
