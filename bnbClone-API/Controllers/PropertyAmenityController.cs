using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyAmenityController : ControllerBase
    {
        private readonly IPropertyAmenityRepo propertyAmenityRepo;

        public PropertyAmenityController(IPropertyAmenityRepo propertyAmenityRepo)
        {
            this.propertyAmenityRepo = propertyAmenityRepo;
        }



        [HttpPost]
        public async Task<IActionResult> AddAmenityToProperty(PropertyAmenityDTO property)
        {
            PropertyAmenity amenity =new PropertyAmenity()
            {
                PropertyId = property.PropertyId,
                AmenityId = property.AmenityId,
                CreatedAt = property.CreatedAt,
            };
            if (property != null)
            {
                await propertyAmenityRepo.AddAsync(amenity);

                return Ok("done");

            }

            return BadRequest("enter data");

        }



        [HttpDelete]
        public async Task<IActionResult> DeleteAmenityFromProperty(int propID, int AmenityID)
        {


           PropertyAmenity amenity=await propertyAmenityRepo.DeleteAsync(propID ,AmenityID);

            return Ok(amenity);

        }




    }
}
