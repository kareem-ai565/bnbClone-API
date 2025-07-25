using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IBookingPaymentRepo : IGenericRepo<BookingPayment>
    {
        Task<IEnumerable<BookingPayment>> GetByBookingIdAsync(int bookingId);
        Task<BookingPayment> GetByTransactionIdAsync(string sessionId);
        Task<IEnumerable<BookingPayment>> GetByGuestIdAsync(int guestId);
        Task<IEnumerable<BookingPayment>> GetByPropertyIdAsync(int propertyId);
        Task<IEnumerable<BookingPayment>> GetSearchPaymentsAsync(string status = null,
                                                                 DateTime? fromDate = null,
                                                                 DateTime? toDate = null,
                                                                 int? guestId = null,
                                                                 int? propertyId = null);
    }
}
