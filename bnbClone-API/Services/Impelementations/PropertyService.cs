using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Services.Implementations
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPropertyRepo _propertyRepo;
        private readonly IMapper _mapper;

        public PropertyService(IUnitOfWork unitOfWork, IPropertyRepo propertyRepo, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            _propertyRepo = propertyRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _propertyRepo.GetAllAsync();
        }

        public async Task<Property?> GetByIdAsync(int id)
        {
            return await _propertyRepo.GetDetailsByIdAsync(id);
        }

        public async Task<Property> AddAsync(Property property)
        {
            await _propertyRepo.AddAsync(property);
            await unitOfWork.SaveAsync();
            return property;
        }

        public async Task<bool> UpdateAsync(Property property)
        {
            var updated = await _propertyRepo.UpdateAsync(property);
            await unitOfWork.SaveAsync();
            return updated != null;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var result = await _propertyRepo.DeleteAsync(id);
            await unitOfWork.SaveAsync();
            return result;
        }

        public async Task<List<Property>> SearchAsync(PropertySearchDto dto)
        {
            return await _propertyRepo.SearchAsync(dto);
        }

        public async Task<Property?> GetByIdWithAmenitiesAsync(int id)
        {
            return await unitOfWork.Context.Properties
                .Include(p => p.PropertyAmenities)
                .ThenInclude(pa => pa.Amenity)
                .FirstOrDefaultAsync(p => p.Id == id);
        }


        public async Task<bool> UpdateStep5AmenitiesAsync(int propertyId, List<int> amenityIds)
        {
            var property = await unitOfWork.Context.Properties
                .Include(p => p.PropertyAmenities)
                .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null)
                return false;

            // Clear old
            property.PropertyAmenities.Clear();

            // Add new
            foreach (var amenityId in amenityIds)
            {
                property.PropertyAmenities.Add(new PropertyAmenity
                {
                    PropertyId = propertyId,
                    AmenityId = amenityId
                });
            }

            await unitOfWork.Context.SaveChangesAsync();
            return true;
        }



    }
}
