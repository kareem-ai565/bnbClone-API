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
using System.Xml.Schema;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyCategoryController : ControllerBase
    {
        private readonly IPropertyCategoryService service;

        public PropertyCategoryController(IPropertyCategoryService service)
        {
            this.service = service;
        }



        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {


            return Ok(await service.GetAllPropertyCategories());

        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllCategoryById(int id)
        {

            return Ok(await service.GetPropertyCategoryById(id));

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
           PropertyCategory property=await service.GetPropertyCategoryById(id);

            if (property != null) {

                await service.DeletePropertyCategory(id);
                return Ok(property);
            
            
            }
            return BadRequest(new { error = "No item with this id enter another id" });

        }





        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] CategoryDTO category)
        {
            if(category != null)
            {

                await  service.AddPropertyCategory(category);

                return Ok(category);

            }


            else
            {
                return BadRequest("No Data Sent");
            }




        }




        [HttpPut]

        public async Task<IActionResult> EditCategory(int id , [FromForm] CategoryDTO category)
        {

            if(id != null  && category!=null)
            {
                await service.EditPropertyCategory(id, category);

                return Ok(category);

            }

            else
            {
                return BadRequest(new { error = "U must Enter valid Id it's required and valid data " });
            }

          
        }
        








    }
}
