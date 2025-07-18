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


    }
}
