using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
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


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            return Ok(await propertyCategoryRepo.DeleteAsync(id));

        }



        [HttpPost]
        public IActionResult AddCategory([FromForm] CategoryDTO category)
        {
            if(category != null)
            {


                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");


                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);

                }

                var FileName = $"{Guid.NewGuid()}{Path.GetExtension(category.IconUrl.FileName)}";

                var FilePath = Path.Combine(FolderPath, FileName);


                using var FileStream = new FileStream(FilePath, FileMode.CreateNew);

                category.IconUrl.CopyTo(FileStream);





                PropertyCategory property = new PropertyCategory()
                {
                    Name = category.Name,
                    Description = category.Description,
                    IconUrl = FileName

                };


                return Ok(property);

            }


            else
            {
                return BadRequest("No Data Sent");
            }




        }




        [HttpPut]

        public async Task<IActionResult> EditCategory(int id , [FromForm] CategoryDTO category)
        {
            PropertyCategory property = await propertyCategoryRepo.GetByIdAsync(id);

            property.Description = category.Description;
            property.Name = category.Name;

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
            var FileName = $"{Guid.NewGuid()}{category.IconUrl.FileName}";

            var FilePath = Path.Combine(FolderPath, FileName);
            using var FileStream = new FileStream(FilePath , FileMode.CreateNew);

            category.IconUrl.CopyTo(FileStream);

            var OldImage = Path.Combine(FolderPath, property.IconUrl);

            System.IO.File.Delete(OldImage);


            property.IconUrl = FileName;



            return Ok(property);
        }
        








    }
}
