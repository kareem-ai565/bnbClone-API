using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IAvailabilityRepo : IGenericRepo<PropertyAvailability>
    {
        Task<IEnumerable<PropertyAvailability>> FindByPropertyIdAsync(int propertyId);
        Task<IEnumerable<PropertyAvailability>> GetAvailabilityByHostIdAsync(int hostId);

    }

}
