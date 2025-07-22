using bnbClone_API.DTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IBookingPayoutService
    {
        public Task<bool> ReleasePayoutToHostAsync(int bookingId);
        public Task<IEnumerable<BookingPayoutResponseDto>> GetAllPayoutsToHostAsync();

    }
}
