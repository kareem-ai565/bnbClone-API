using bnbClone_API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyCategoryController : ControllerBase
    {
        private readonly IPropertyCategoryRepo propertyCategoryRepo;



        public PropertyCategoryController(IPropertyCategoryRepo propertyCategoryRepo)
        {
            this.propertyCategoryRepo = propertyCategoryRepo;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {

            return Ok(await propertyCategoryRepo.GetAllAsync());

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCategoryById(int id)
        {

            return Ok(await propertyCategoryRepo.GetByIdAsync(id));

        }


        








    }
}
