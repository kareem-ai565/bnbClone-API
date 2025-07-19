using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
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
        private readonly IUnitOfWork unitOfWork;

        public PropertyAmenityController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        [HttpPost]
        public async Task<IActionResult> AddAmenityToProperty(PropertyAmenityDTO property)
        {
            PropertyAmenity amenity = new PropertyAmenity()
            {
                PropertyId = property.PropertyId,
                AmenityId = property.AmenityId,
                CreatedAt = property.CreatedAt,
            };
            if (property != null)
            {
                await unitOfWork.PropAmenities.AddAsync(amenity);
                await unitOfWork.SaveAsync();

                return Ok("done");

            }

            return BadRequest("enter data");

        }



        [HttpDelete]
        public async Task<IActionResult> DeleteAmenityFromProperty(int propID, int AmenityID)
        {


            PropertyAmenity amenity = await unitOfWork.PropAmenities.DeleteAsync(propID, AmenityID);

            await unitOfWork.SaveAsync();

            return Ok(amenity);

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmenitiesOfProperty(int id)
        {
          List<PropertyAmenity> properties = await  unitOfWork.PropAmenities.GetAmenitiesOfProperty(id);

            return Ok(properties);

        }


    }
}
