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

            CreateMap<PropertyAvailability, PropertyAvailabilityDto>();
            CreateMap<Property, PropertyDetailsDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
                .ForMember(dest => dest.AmenityNames, opt => opt.MapFrom(src => src.PropertyAmenities.Select(pa => pa.Amenity.Name)))
                .ForMember(dest => dest.HostName, opt => opt.MapFrom(src => src.Host.User.FirstName + " " + src.Host.User.LastName))
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType))
                .ForMember(dest => dest.AvailabilityDates, opt => opt.MapFrom(src => src.Availabilities));




            CreateMap<Review, ReviewDto>()
                .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.Reviewer.FirstName + " " + src.Reviewer.LastName));

            CreateMap<Property, PropertyDetailsDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
                .ForMember(dest => dest.AmenityNames, opt => opt.MapFrom(src => src.PropertyAmenities.Select(pa => pa.Amenity.Name)))
                .ForMember(dest => dest.HostName, opt => opt.MapFrom(src => src.Host.User.FirstName + " " + src.Host.User.LastName))
                .ForMember(dest => dest.PropertyTypeName, opt => opt.MapFrom(src => src.PropertyType))
                .ForMember(dest => dest.AvailabilityDates, opt => opt.MapFrom(src => src.Availabilities))
                .ForMember(dest => dest.Reviews, opt => opt.MapFrom(src => src.Reviews));  


        }

    }
}
