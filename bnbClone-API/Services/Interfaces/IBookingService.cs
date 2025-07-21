using bnbClone_API.DTOs;
using bnbClone_API.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace bnbClone_API.Services.Interfaces
{
    public interface IBookingService
    {
        public Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
        public Task<BookingResponseDto> GetBookingByIdAsync(int id);
        public Task<int> AddBooking(int userId,BookingCreateDto createBookingDto);
        public Task<int> DeleteBooking(int bookingid);
        public Task<int> UpdateBooking(int bookingid, BookingUpdateDto bookingUpdateDto);
        public Task<int> UpdateBookingStatusAsync(int bookingId, BookingStatusUpdateDto bookingStatusUpdateDto);
        // new 
        Task<IEnumerable<BookingResponseDto>> GetBookingsByGuestAsync(int guestId);
        Task<IEnumerable<BookingResponseDto>> GetBookingsByPropertyAsync(int propertyId);
        public Task<int> UpdateBookingCheckInStatusAsync(int bookingId , BookingCheckInStatusUpdate bookingCheckInStatusUpdate);
        public Task<int> UpdateBookingCheckOutStatusAsync(int bookingId , BookingCheckOutStatusUpdate bookingCheckOutStatusUpdate);
        Task<IEnumerable<BookingResponseDto>> SearchBookingsAsync(BookingSearchDto searchDto);
        Task<BookingStatsDto> GetBookingStatsAsync();
        Task<Booking> GetGuestByBookingIdAsync(int bookingId);



    }
}
