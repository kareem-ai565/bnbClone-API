using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;

namespace bnbClone_API.Helpers.MappingProfiles
{
    public class PropertyProfile : Profile
    {
        public PropertyProfile()
        {
            // Create
            CreateMap<CreatePropertyDto, Property>();
            CreateMap<Property, CreatePropertyDto>();

            // Update
            CreateMap<UpdatePropertyDto, Property>();
            CreateMap<Property, UpdatePropertyDto>();

            // Details (only Property ➝ DTO)
            CreateMap<Property, PropertyDetailsDto>();
        }
        
    }
}
