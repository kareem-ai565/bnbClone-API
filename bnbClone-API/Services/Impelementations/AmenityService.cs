using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace bnbClone_API.Services.Impelementations
{
    public class AmenityService:IAmenityService
    {
        private readonly IUnitOfWork unitOfWork;

        public AmenityService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



       
        public async Task<IEnumerable<Amenity>> GetAmenities()
        {
            return await unitOfWork._Amenities.GetAllAsync();


        }


        public async Task<Amenity> GetAmenityById(int id)
        {
            return await unitOfWork._Amenities.GetByIdAsync(id);

        }






        public async Task<Amenity> AddAmenity([FromForm] AmenityDTO amenity)
        {
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");

            if (!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);

            var FileName = $"{Guid.NewGuid()}{Path.GetExtension(amenity.IconUrl.FileName)}";


            var FilePath = Path.Combine(FolderPath, FileName);


            using var FileStream = new FileStream(FilePath, FileMode.CreateNew);

            amenity.IconUrl.CopyTo(FileStream);

            Amenity amenity1 = new Amenity()
            {
                Name = amenity.Name,
                Category = amenity.Category,
                IconUrl = FileName,
            };


            await unitOfWork._Amenities.AddAsync(amenity1);
           await unitOfWork.SaveAsync();

            return amenity1;
        }




        public async Task<Amenity> EditAmenity(int id, [FromForm] AmenityDTO amenity)
        {
            Amenity amenity1 = await unitOfWork._Amenities.GetByIdAsync(id);

                amenity1.Name = amenity.Name;
                amenity1.Category = amenity.Category;

                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Images");

                var FileName = $"{Guid.NewGuid()}{Path.GetExtension(amenity.IconUrl.FileName)}";

                var FilePath = Path.Combine(FolderPath, FileName);


                using var fileStream = new FileStream(FilePath, FileMode.CreateNew);


                amenity.IconUrl.CopyTo(fileStream);

                var OldImage = Path.Combine(FolderPath, amenity1.IconUrl);


                if (OldImage != null)
                {
                    System.IO.File.Delete(OldImage);
                }

                amenity1.IconUrl = FileName;



                await unitOfWork.SaveAsync();


                return amenity1;

            
        }




        public async Task<Amenity> DeleteAmenity(int id)
        {
            Amenity amenity = await unitOfWork._Amenities.GetByIdAsync(id);



           

                await unitOfWork._Amenities.DeleteAsync(id);
                await unitOfWork.SaveAsync();
                return amenity;

            

            
        }
    }
}
