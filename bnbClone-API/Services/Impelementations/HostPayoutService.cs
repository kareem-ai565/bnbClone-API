using bnbClone_API.DTOs;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Impelementations
{
    public class HostPayoutService: IHostPayoutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HostPayoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<HostPayoutResponseDto>> GetAllAsync()
        {
            var payouts = await _unitOfWork.HostPayoutRepo.GetAllAsync();
            return payouts.Select(p => new HostPayoutResponseDto
            {
                Id = p.Id,
                HostId = p.HostId,
                Amount = p.Amount,
                Status = p.Status,
                PayoutMethod = p.PayoutMethod,
                TransactionId = p.TransactionId,
                CreatedAt = p.CreatedAt,
                ProcessedAt = p.ProcessedAt,
                HostFullName = $"{p.Host?.User?.FirstName} {p.Host?.User?.LastName}"
            });
        }

        public async Task<IEnumerable<HostPayoutResponseDto>> GetByHostIdAsync(int hostId)
        {
            var payouts = await _unitOfWork.HostPayoutRepo.GetByHostIdAsync(hostId);
            return payouts.Select(p => new HostPayoutResponseDto
            {
                Id = p.Id,
                HostId = p.HostId,
                Amount = p.Amount,
                Status = p.Status,
                PayoutMethod = p.PayoutMethod,
                TransactionId = p.TransactionId,
                CreatedAt = p.CreatedAt,
                ProcessedAt = p.ProcessedAt,
                HostFullName = $"{p.Host?.User?.FirstName} {p.Host?.User?.LastName}"
            });
        }
        public async Task<HostPayoutResponseDto> GetByIdAsync(int id)
        {
            var payout = await _unitOfWork.HostPayoutRepo.GetByIdAsync(id);
            if (payout == null) return null;
            return new HostPayoutResponseDto
            {
                Id = payout.Id,
                HostId = payout.HostId,
                Amount = payout.Amount,
                Status = payout.Status,
                PayoutMethod = payout.PayoutMethod,
                TransactionId = payout.TransactionId,
                CreatedAt = payout.CreatedAt,
                ProcessedAt = payout.ProcessedAt,
                HostFullName = $"{payout.Host?.User?.FirstName} {payout.Host?.User?.LastName}"
            };
        }
    }
}

