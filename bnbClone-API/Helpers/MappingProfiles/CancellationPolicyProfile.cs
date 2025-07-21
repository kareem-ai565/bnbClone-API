using AutoMapper;
using bnbClone_API.DTOs.CancelationPolcyDts;
using bnbClone_API.Models;

namespace bnbClone_API.Helpers.MappingProfiles
{
    public class CancellationPolicyProfile : Profile
    {
        public CancellationPolicyProfile()
        {
            CreateMap<CancellationPolicy, CancellationPolicyDto>();
            CreateMap<CancellationPolicyCreateDto, CancellationPolicy>();
            CreateMap<CancellationPolicyUpdateDto, CancellationPolicy>();

        }
    }
    
}
