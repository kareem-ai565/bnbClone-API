using bnbClone_API.DTOs;
using bnbClone_API.Models;

namespace bnbClone_API.Services.Interfaces
{
    public interface IPropertyAmenityService
    {
        Task<PropertyAmenity> AddAmenityAndProperty(PropertyAmenityDTO property);

        Task<PropertyAmenity> deleteAmenitywithProperty(int propID, int AmenityID);

        Task<List<PropertyAmenity>> GetAmenityOfProperty(int id);
    }
}
