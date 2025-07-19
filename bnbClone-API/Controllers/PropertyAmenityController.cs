using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyAmenityController : ControllerBase
    {
        private readonly IPropertyAmenityService amenityService;

        public PropertyAmenityController(IPropertyAmenityService amenityService)
        {
            this.amenityService = amenityService;
        }


        
        [HttpPost]
        public async Task<IActionResult> AddAmenityToProperty(PropertyAmenityDTO property)
        {
            PropertyAmenity property1=await amenityService.AddAmenityAndProperty(property);

            if (property1 != null) {

                return Ok(property1);
            }
            else
            {
                return BadRequest(new {error= "Enter Data There is no data to add it" });
            }
        }



        [HttpDelete]
        public async Task<IActionResult> DeleteAmenityFromProperty(int propID, int AmenityID)
        {

           PropertyAmenity amenity=  await amenityService.deleteAmenitywithProperty(propID , AmenityID);

            if (amenity != null) {
                return Ok(amenity);
            }


            return BadRequest(new { error = "there is no amenityProp with data which u enter" });



        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmenitiesOfProperty(int id)
        {
           List<PropertyAmenity> AllProperties= await amenityService.GetAmenityOfProperty(id);

            if (AllProperties != null) {
                return Ok(AllProperties);

            }

            return BadRequest(new { error = "No property with this ID Enter another Id" });



        }


    }
}
