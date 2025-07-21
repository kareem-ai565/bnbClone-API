using bnbClone_API.DTOs.HostDTOs;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class HostService : IHostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<HostDto>> GetAllHostsAsync()
        {
            var hosts = await _unitOfWork.Hosts.GetAllAsync();

            return hosts.Select(h => new HostDto
            {
                Id = h.Id,
                UserId = h.UserId,
                AboutMe = h.AboutMe,
                Work = h.Work,
                Rating = h.Rating,
                TotalReviews = h.TotalReviews,
                Education = h.Education,
                Languages = h.Languages,
                IsVerified = h.IsVerified,
                TotalEarnings = h.TotalEarnings,
                AvailableBalance = h.AvailableBalance,
                StripeAccountId = h.StripeAccountId,
                DefaultPayoutMethod = h.DefaultPayoutMethod,
                PayoutAccountDetails = h.PayoutAccountDetails,
                LivesIn = h.LivesIn,
                DreamDestination = h.DreamDestination,
                FunFact = h.FunFact,
                Pets = h.Pets,
                ObsessedWith = h.ObsessedWith,
                SpecialAbout = h.SpecialAbout
            });
        }

        public async Task<HostDto?> GetHostByIdAsync(int hostId)
        {
            var host = await _unitOfWork.Hosts.GetHostWithUserAsync(hostId);
            return host == null ? null : MapToDto(host);
        }

        public async Task<HostDto?> GetHostByUserIdAsync(int userId)
        {
            var host = await _unitOfWork.Hosts.GetByUserIdAsync(userId);
            return host == null ? null : MapToDto(host);
        }

        public async Task<bool> UpdateHostByUserIdAsync(int userId, HostUpdateDto dto)
        {
            var host = await _unitOfWork.Hosts.GetByUserIdAsync(userId);
            if (host == null) return false;

            host.AboutMe = dto.AboutMe;
            host.Work = dto.Work;
            host.Education = dto.Education;
            host.Languages = dto.Languages;
            host.LivesIn = dto.LivesIn;
            host.DreamDestination = dto.DreamDestination;
            host.FunFact = dto.FunFact;
            host.Pets = dto.Pets;
            host.ObsessedWith = dto.ObsessedWith;
            host.SpecialAbout = dto.SpecialAbout;
            host.DefaultPayoutMethod = dto.DefaultPayoutMethod;
            host.PayoutAccountDetails = dto.PayoutAccountDetails;

            await _unitOfWork.CompleteAsync();
            return true;
        }

        private HostDto MapToDto(Models.Host h)
        {
            return new HostDto
            {
                Id = h.Id,
                UserId = h.UserId,
                AboutMe = h.AboutMe,
                Work = h.Work,
                Rating = h.Rating,
                TotalReviews = h.TotalReviews,
                Education = h.Education,
                Languages = h.Languages,
                IsVerified = h.IsVerified,
                TotalEarnings = h.TotalEarnings,
                AvailableBalance = h.AvailableBalance,
                StripeAccountId = h.StripeAccountId,
                DefaultPayoutMethod = h.DefaultPayoutMethod,
                PayoutAccountDetails = h.PayoutAccountDetails,
                LivesIn = h.LivesIn,
                DreamDestination = h.DreamDestination,
                FunFact = h.FunFact,
                Pets = h.Pets,
                ObsessedWith = h.ObsessedWith,
                SpecialAbout = h.SpecialAbout
            };
        }
    }
}
