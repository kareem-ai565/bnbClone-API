using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IBookingRepo:IGenericRepo<Booking>
    {
        public Task<IEnumerable<Booking>> GetByGuestIdAsync(int id);
         public Task<IEnumerable<Booking>> GetByPropertyIdAsync(int id);
         public Task<IEnumerable<Booking>> GetSearchBookingAsync(string status = null,
                                                                DateTime? fromDate = null,
                                                                DateTime? toDate = null,
                                                                int? guestId = null,
                                                                int? propertyId = null);
        Task<Booking> GetGuestByBookingIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetByHostIdAsync(int hostId);

    }
}
