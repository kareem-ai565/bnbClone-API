using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.UnitOfWork;
using System.Threading.Tasks;

namespace bnbClone_API.Services.Impelementations
{
    public class PropertyAmenityService
    {

        private readonly IUnitOfWork unitOfWork;

        public PropertyAmenityService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }




        public async Task<PropertyAmenity> AddAmenityAndProperty(PropertyAmenityDTO property)
        {
            PropertyAmenity amenity = new PropertyAmenity()
            {
                PropertyId = property.PropertyId,
                AmenityId = property.AmenityId,
                CreatedAt = property.CreatedAt,
            };
            
           var propertyAmenity = await unitOfWork.PropAmenities.AddAsync(amenity);
              await unitOfWork.SaveAsync();

            return propertyAmenity;

        }



        public  async Task<PropertyAmenity> deleteAmenitywithProperty(int propID, int AmenityID)
        {

            PropertyAmenity propertyAmenity = await unitOfWork.PropAmenities.DeleteAsync(propID, AmenityID);

            await unitOfWork.SaveAsync();

            return propertyAmenity;

        }



        public async Task<List<PropertyAmenity>> GetAmenityOfProperty(int id)
        {
            List<PropertyAmenity> properties = await unitOfWork.PropAmenities.GetAmenitiesOfProperty(id);

            return properties;

        }
    }
}
