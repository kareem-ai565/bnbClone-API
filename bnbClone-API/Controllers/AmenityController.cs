using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityRepo amenityRepo;

        public AmenityController(IAmenityRepo amenityRepo)
        {
            this.amenityRepo = amenityRepo;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllAmenities()
        {

          return  Ok( await amenityRepo.GetAllAsync());

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmenityById(int id)
        {

            return Ok(await amenityRepo.GetByIdAsync(id));

        }





        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {

            return Ok(await amenityRepo.DeleteAsync(id));

        }



        [HttpPost]
        public async Task<IActionResult> AddAmenity([FromForm] AmenityDTO amenity)
        {

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");

            Amenity amenity1 = new Amenity()
            {
                Id = amenity.Id,
                Name = amenity.Name,
                Category = amenity.Category,
                IconUrl = amenity.IconUrl,
            };


            await amenityRepo.AddAsync(amenity1);

            return Ok(amenity);

        }






        //[HttpPut]
        //public IActionResult UpdateAmenity()
        //{

        //}









    }
}
