using AutoMapper;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Interfaces;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PropertyImageService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyIdAsync(int propertyId)
        {
            var images = await _unitOfWork.PropertyImageRepo.GetImagesByPropertyIdAsync(propertyId);
            return _mapper.Map<IEnumerable<PropertyImageDto>>(images);
        }

        public async Task<PropertyImageDto> AddImageAsync(int propertyId, CreatePropertyImageDto dto)
        {
            var image = _mapper.Map<PropertyImage>(dto);
            image.PropertyId = propertyId;

            var added = await _unitOfWork.PropertyImageRepo.AddAsync(image);
            await _unitOfWork.SaveAsync();  

            return _mapper.Map<PropertyImageDto>(added);
        }

        public async Task<bool> DeleteImageAsync(int imageId)
        {
            var success = await _unitOfWork.PropertyImageRepo.DeleteAsync(imageId);

            if (success)
                await _unitOfWork.SaveAsync(); 

            return success;
        }
    }
}
