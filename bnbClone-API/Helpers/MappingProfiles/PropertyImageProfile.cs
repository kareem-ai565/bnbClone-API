using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.Models;

namespace bnbClone_API.Helpers.MappingProfiles
{
    public class PropertyImageProfile : Profile
    {
        public PropertyImageProfile()
        {
            CreateMap<CreatePropertyImageDto, PropertyImage>();
            CreateMap<PropertyImage, PropertyImageDto>();
        }
    }
}
