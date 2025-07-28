using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenityService service;

        public AmenityController(IAmenityService service)
        {
            this.service = service;
           
        }




       [Authorize]

        [HttpGet]
        public async Task<IActionResult> GetAllAmenities()
        {


          return  Ok(await service.GetAmenities());

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAmenityById(int id)
        {

            return Ok(await service.GetAmenityById(id));

        }





        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAmenity(int id)
        {

            Amenity amenity = await service.GetAmenityById(id);



            if (amenity != null)
            {

               await service.DeleteAmenity(id);
                return Ok(amenity);

            }

            else
            {
                return BadRequest("Enter Id Found");
            }


        }



        [Consumes("multipart/form-data")]

        [HttpPost]
        public async Task<IActionResult> AddAmenity([FromForm] AmenityDTO amenity)
        {
            if(amenity != null)
            {

              await  service.AddAmenity(amenity);


                return Ok(amenity);

            }
            else
            {
                return BadRequest(new { error = "U must enter all field which are required" });
            }
           
            

        }





        [Consumes("multipart/form-data")]
        [HttpPut]
        public async Task<IActionResult> UpdateAmenity(int id, [FromForm]  AmenityDTO amenity)
        {
            

            if (id != null  && amenity !=null )
            {
                await service.EditAmenity(id, amenity);
                return Ok(amenity);

            }
            

            return BadRequest(new {error= "U must enter all field which are required and correct id" });
            
      
        }

    }
}
