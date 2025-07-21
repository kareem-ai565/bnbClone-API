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

        public Task<bool> ReleasePayoutToHostAsync(int bookingId)
        {
            throw new NotImplementedException();
        }
        //public async Task<bool> ReleasePayoutToHostAsync(int bookingId)
        //{
        //    var booking = await _unitOfWork.BookingRepo.GetBookingWithPropertyAsync(bookingId);
        //    if (booking == null || booking.CheckInStatus.ToLower() != "completed")
        //        return false;

        //    var payout = await _unitOfWork.GenericRepo<BookingPayout>()
        //        .FindFirstAsync(p => p.BookingId == bookingId && p.Status == "Pending");

        //    if (payout == null) return false;

        //    // Create host payout record
        //    var hostPayout = new HostPayout
        //    {
        //        HostId = booking.Property.OwnerId,
        //        Amount = payout.Amount,
        //        Status = "Completed",
        //        PayoutMethod = "Bank Transfer", // default or from admin form
        //        PayoutAccountDetails = "EncryptedDataHere",
        //        TransactionId = Guid.NewGuid().ToString(),
        //        Notes = $"Payout for booking #{booking.Id}",
        //        ProcessedAt = DateTime.UtcNow
        //    };

        //    payout.Status = "Completed";

        //    await _unitOfWork.GenericRepo<HostPayout>().AddAsync(hostPayout);
        //    await _unitOfWork.SaveAsync();

        //    return true;
        //}
    }
}
