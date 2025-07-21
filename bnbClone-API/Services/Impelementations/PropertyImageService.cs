using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.DTOs.PropertyDtos;


namespace bnbClone_API.Services.Implementations
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IPropertyImageRepo _imageRepo;
        private readonly IMapper _mapper;

        public PropertyImageService(IPropertyImageRepo imageRepo, IMapper mapper)
        {
            _imageRepo = imageRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyIdAsync(int propertyId)
        {
            var images = await _imageRepo.GetImagesByPropertyIdAsync(propertyId);
            return _mapper.Map<IEnumerable<PropertyImageDto>>(images);
        }

        public async Task<PropertyImageDto> AddImageAsync(int propertyId, CreatePropertyImageDto dto)
        {
            var image = _mapper.Map<PropertyImage>(dto);
            image.PropertyId = propertyId;
            var added = await _imageRepo.AddAsync(image);
            return _mapper.Map<PropertyImageDto>(added);
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            return await _imageRepo.DeleteAsync(imageId);
        }
    }
}
