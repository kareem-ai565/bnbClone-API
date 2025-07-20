using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Services.Impelementations
{
    public class PropertyCategoryService:IPropertyCategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public PropertyCategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<PropertyCategory>> GetAllPropertyCategories()
        {
           IEnumerable<PropertyCategory> categories=  await unitOfWork.PropCategory.GetAllAsync();

            return categories;  

        }


        public async Task<PropertyCategory> GetPropertyCategoryById(int id)
        {
            PropertyCategory category = await unitOfWork.PropCategory.GetByIdAsync(id);
            return category;

        }



        public async Task<PropertyCategory> AddPropertyCategory([FromForm] CategoryDTO category)
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

            await unitOfWork.PropCategory.AddAsync(property);

            await unitOfWork.SaveAsync();


            return property;


        }



        public async Task<PropertyCategory> EditPropertyCategory(int id, [FromForm] CategoryDTO category)
        {

            PropertyCategory property = await unitOfWork.PropCategory.GetByIdAsync(id);

            property.Description = category.Description;
            property.Name = category.Name;

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");
            var FileName = $"{Guid.NewGuid()}{category.IconUrl.FileName}";

            var FilePath = Path.Combine(FolderPath, FileName);
            using var FileStream = new FileStream(FilePath, FileMode.CreateNew);

            category.IconUrl.CopyTo(FileStream);

            var OldImage = Path.Combine(FolderPath, property.IconUrl);

            System.IO.File.Delete(OldImage);


            property.IconUrl = FileName;


            await unitOfWork.SaveAsync();


            return property;

        }




        public async Task<PropertyCategory> DeletePropertyCategory(int id)
        {
            PropertyCategory property = await unitOfWork.PropCategory.GetByIdAsync(id);
             await unitOfWork.PropCategory.DeleteAsync(id);

            return property;
        }
    }
}
