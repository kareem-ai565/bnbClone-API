using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");

            if(!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            var FileName = $"{Guid.NewGuid()}{Path.GetExtension(amenity.IconUrl.FileName)}";


            var FilePath=Path.Combine(FolderPath, FileName);


            using var FileStream = new FileStream(FilePath, FileMode.CreateNew);

            amenity.IconUrl.CopyTo(FileStream);

            Amenity amenity1 = new Amenity()
            {
                Name = amenity.Name,
                Category = amenity.Category,
                IconUrl = FileName,
            };


            await amenityRepo.AddAsync(amenity1);

            return Ok(amenity);

        }






        [HttpPut]
        public async Task<IActionResult> UpdateAmenity(int id, [FromForm]  AmenityDTO amenity)
        {

            Amenity amenity1 =await amenityRepo.GetByIdAsync(id);

            if(amenity1 != null)
            {


                amenity1.Name = amenity.Name;
                amenity1.Category = amenity.Category;

                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");

                var FileName = $"{Guid.NewGuid()}{Path.GetExtension(amenity.IconUrl.FileName)}";

                var FilePath = Path.Combine(FolderPath, FileName);


                using var fileStream = new FileStream(FilePath, FileMode.CreateNew);


                amenity.IconUrl.CopyTo(fileStream);


                amenity1.IconUrl = FileName;


                var OldImage = Path.Combine(FolderPath, amenity1.IconUrl);


                if(OldImage != null)
                {
                    System.IO.File.Delete(OldImage);
                }



                //await amenityRepo.UpdateAsync(amenity);



                return Ok(amenity1);

            }


            return BadRequest();
            
      
        }









    }
}
