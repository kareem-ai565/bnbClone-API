using AutoMapper;
using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Impelementations
{
    // Services/CancellationPolicyService.cs
    public class CancellationPolicyService : ICancellationPolicyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CancellationPolicyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CancellationPolicyDto>> GetAllAsync()
        {
            var policies = await _unitOfWork.CancellationPolicies.GetAllAsync();
            return _mapper.Map<IEnumerable<CancellationPolicyDto>>(policies);
        }

        public async Task<CancellationPolicyDto?> GetByIdAsync(int id)
        {
            var policy = await _unitOfWork.CancellationPolicies.GetByIdAsync(id);
            return policy == null ? null : _mapper.Map<CancellationPolicyDto>(policy);
        }

        public async Task<CancellationPolicyDto> CreateAsync(CancellationPolicyCreateDto dto)
        {
            var policy = _mapper.Map<CancellationPolicy>(dto);
            await _unitOfWork.CancellationPolicies.AddAsync(policy);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<CancellationPolicyDto>(policy);
        }

        public async Task<bool> UpdateAsync(int id, CancellationPolicyUpdateDto dto)
        {
            var policy = await _unitOfWork.CancellationPolicies.GetByIdAsync(id);
            if (policy == null) return false;

            _mapper.Map(dto, policy);
            await _unitOfWork.CancellationPolicies.UpdateAsync(policy);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _unitOfWork.CancellationPolicies.ExistsAsync(id);
            if (!exists) return false;

            await _unitOfWork.CancellationPolicies.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return true;
        }

    }

}
