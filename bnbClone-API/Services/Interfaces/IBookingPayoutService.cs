namespace bnbClone_API.Services.Interfaces
{
    public interface IBookingPayoutService
    {
        Task<bool> ReleasePayoutToHostAsync(int bookingId);
    }
}
