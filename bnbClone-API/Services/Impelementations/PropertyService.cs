using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Interfaces;

namespace bnbClone_API.Services.Implementations
{
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepo _propertyRepo;
        private readonly IMapper _mapper;

        public PropertyService(IPropertyRepo propertyRepo, IMapper mapper)
        {
            _propertyRepo = propertyRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _propertyRepo.GetAllAsync();
        }

        public async Task<Property?> GetByIdAsync(int id)
        {
            return await _propertyRepo.GetByIdAsync(id);
        }

        public async Task<Property> AddAsync(Property property)
        {
            await _propertyRepo.AddAsync(property);
            return property;
        }

        public async Task<bool> UpdateAsync(Property property)
        {
            var updated = await _propertyRepo.UpdateAsync(property);
            return updated != null;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            return await _propertyRepo.DeleteAsync(id);
        }

        public async Task<List<Property>> SearchAsync(PropertySearchDto dto)
        {
            return await _propertyRepo.SearchAsync(dto);
        }


    }
}
