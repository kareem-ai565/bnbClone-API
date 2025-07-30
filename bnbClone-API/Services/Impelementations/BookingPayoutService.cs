using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Impelementations
{
    public class BookingPayoutService : IBookingPayoutService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingPayoutService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<BookingPayoutResponseDto>> GetAllPayoutsToHostAsync()
        {
            var payouts = await _unitOfWork.BookingPayoutRepo.GetAllAsync();
            return payouts.Select(p => new BookingPayoutResponseDto
            {
                Id = p.Id,
                Amount = p.Amount,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                BookingId = p.BookingId,
                PropertyTitle = p.Booking?.Property?.Title ?? "N/A",
                GuestFullName = $"{p.Booking?.Guest?.FirstName} {p.Booking?.Guest?.LastName}",
                HostFullName = $"{p.Booking?.Property?.Host?.User?.FirstName} {p.Booking?.Property?.Host?.User?.LastName}"
            });

        }

        public async Task<bool> ReleasePayoutToHostAsync(int bookingId)
            {
                var booking = await _unitOfWork.BookingRepo.GetGuestByBookingIdAsync(bookingId);
                if (booking == null || booking.CheckInStatus.ToLower() != "completed")
                    return false;

                var payout = await _unitOfWork.BookingPayoutRepo.FindFirstAsync(p => p.BookingId == bookingId && p.Status == "Pending");

                if (payout == null) return false;

                // Create host payout record
                var hostPayout = new HostPayout
                {
                    HostId = booking.Property.HostId,
                    Amount = payout.Amount,
                    Status = "Completed",
                    PayoutMethod = "Bank Transfer", // default or from admin form
                    PayoutAccountDetails = "EncryptedDataHere",
                    TransactionId = Guid.NewGuid().ToString(),
                    Notes = $"Payout for booking #{booking.Id}",
                    ProcessedAt = DateTime.UtcNow
                };

                payout.Status = "Completed";
            await _unitOfWork.BookingPayoutRepo.UpdateAsync(payout);
            await _unitOfWork.HostPayoutRepo.AddAsync(hostPayout);
                await _unitOfWork.SaveAsync();

                return true;
            }

    }
}
