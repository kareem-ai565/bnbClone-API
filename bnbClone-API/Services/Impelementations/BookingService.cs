using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using System.Numerics;

namespace bnbClone_API.Services.Impelementations
{

    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork unitOfWork;

        public  BookingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<int> AddBooking(BookingCreateDto createBookingDto)
        {
            var booking = new Booking()
            {
                PropertyId = createBookingDto.PropertyId,
                StartDate = createBookingDto.StartDate,
                EndDate = createBookingDto.EndDate,
                TotalGuests = createBookingDto.TotalGuests,
                Status = BookingStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow,
                PromotionId = createBookingDto.PromotionId ?? 0
            };
            await unitOfWork.BookingRepo.AddAsync(booking);
            await unitOfWork.SaveChanges();
            return booking.Id;

        }

        public async Task<int> DeleteBooking(int bookingid)
        {
            var booking = await unitOfWork.BookingRepo.GetByIdAsync(bookingid);
            if (booking == null)
            {
                return 0;
            }
            else
            {
                await unitOfWork.BookingRepo.DeleteAsync(bookingid);
                await unitOfWork.SaveChanges();
                return booking.Id;
            }
        }

        public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync()
        {
            var bookings = await unitOfWork.BookingRepo.GetAllAsync();
            var result = new List<BookingResponseDto>();

            foreach (var booking in bookings) {
                result.Add(new BookingResponseDto()
                {
                    Id = booking.Id,
                    GuestName = booking.Guest.FirstName + " " + booking.Guest.LastName,
                    PropertyTitle = booking.Property.Title,
                    PropertyAddress = booking.Property.Address,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    CheckInStatus = booking.CheckInStatus,
                    CheckOutStatus = booking.CheckOutStatus,
                    TotalAmount = booking.TotalAmount,
                    PromotionId = booking.PromotionId == 0 ? null : booking.PromotionId,
                    Status = Enum.TryParse<BookingStatus>(booking.Status, out var statusEnum) ? statusEnum : BookingStatus.Pending,
                    CreatedAt = booking.CreatedAt,

                });
            }
            return result;
        }

        public async Task<BookingResponseDto> GetBookingByIdAsync(int id)
        {
            var booking = await unitOfWork.BookingRepo.GetByIdAsync(id);
            if (booking == null)
                return null;
            return new BookingResponseDto()
            {
                Id = booking.Id,
                GuestName = booking.Guest.FirstName + " " + booking.Guest.LastName,
                PropertyTitle = booking.Property.Title,
                PropertyAddress = booking.Property.Address,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                CheckInStatus = booking.CheckInStatus,
                CheckOutStatus = booking.CheckOutStatus,
                TotalAmount = booking.TotalAmount,
                PromotionId = booking.PromotionId == 0 ? null : booking.PromotionId,
                Status = Enum.TryParse<BookingStatus>(booking.Status, out var statusEnum) ? statusEnum : BookingStatus.Pending,
                CreatedAt = booking.CreatedAt,

            };


        }

        public async Task<int> UpdateBooking(int bookingid, BookingUpdateDto bookingUpdateDto)
        {
            var booking = await unitOfWork.BookingRepo.GetByIdAsync(bookingid);
            if (booking == null)
            {
                return 0;
            }
            booking.StartDate = bookingUpdateDto.StartDate;
            booking.EndDate = bookingUpdateDto.EndDate; 
            booking.TotalGuests = bookingUpdateDto.TotalGuests;
            booking.PromotionId = bookingUpdateDto.PromotionId ?? 0;
            booking.UpdatedAt = DateTime.UtcNow;

            await unitOfWork.BookingRepo.UpdateAsync(booking);
            await unitOfWork.SaveChanges();
            return booking.Id;

        }

        public async Task<int> UpdateBookingStatusAsync(int bookingId, BookingStatusUpdateDto bookingStatusUpdateDto)
        {
            var booking = await unitOfWork.BookingRepo.GetByIdAsync(bookingId);
            if (booking == null)
            {
                return 0;
            }
            booking.Status = bookingStatusUpdateDto.Status.ToString();
            booking.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.BookingRepo.UpdateAsync(booking);
            await unitOfWork.SaveChanges();
            return booking.Id;
        }
    }
}
