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
            //var property = await unitOfWork.PropertyRepo.GetByIdAsync(createBookingDto.PropertyId);
            //if (property == null)
            //{
            //    throw new ArgumentException("Property not found.");
            //}
            // Validate guest count
            //if (createBookingDto.TotalGuests > property.MaxGuests)
            //    throw new Exception($"Total guests exceed property max capacity ({property.MaxGuests}).");

            // Validate overlapping dates
            var isOverlapping = await unitOfWork.BookingRepo
                .FindAnyConAsync(b =>
                    b.PropertyId == createBookingDto.PropertyId &&
                    b.Status != BookingStatus.Cancelled.ToString() && // ignore cancelled
                    (
                        (createBookingDto.StartDate >= b.StartDate && createBookingDto.StartDate < b.EndDate) ||
                        (createBookingDto.EndDate > b.StartDate && createBookingDto.EndDate <= b.EndDate) ||
                        (createBookingDto.StartDate <= b.StartDate && createBookingDto.EndDate >= b.EndDate)
                    )
                );

            if (isOverlapping)
                throw new Exception("This property is already booked during the selected dates.");
            //var nights = (createBookingDto.EndDate - createBookingDto.StartDate).TotalDays;
            //if (nights <= 0)
            //{
            //    throw new ArgumentException("End date must be after start date.");
            //}
            //if (createBookingDto.TotalGuests <= 0)
            //{
            //    throw new ArgumentException("Total guests must be greater than zero.");
            //}

            //decimal baseAmount = property.PricePerNight * nights;
            //decimal discount = 0;
            //if (createBookingDto.PromotionId.HasValue && createBookingDto.PromotionId.Value > 0)
            //{
            //    var promo = await unitOfWork.PromotionRepo.GetByIdAsync(createBookingDto.PromotionId.Value);
            //    if (promo != null)
            //    {
            //        discount = baseAmount * (promo.DiscountPercentage / 100m);
            //    }
            //}

            //decimal finalAmount = baseAmount - discount;

            var booking = new Booking()
            {
                PropertyId = createBookingDto.PropertyId,
                StartDate = createBookingDto.StartDate,
                EndDate = createBookingDto.EndDate,
                TotalGuests = createBookingDto.TotalGuests,
                Status = BookingStatus.Pending.ToString(),
                //TotalAmount = finalAmount,
                CreatedAt = DateTime.UtcNow,
                PromotionId = createBookingDto.PromotionId ?? 0
            };
            await unitOfWork.BookingRepo.AddAsync(booking);
            await unitOfWork.SaveChanges();
            // Host Notification
            //var hostId = property.HostId;
            //var guestId = booking.GuestId;

            //var notification = new Notification
            //{
            //    UserId = hostId,
            //    SenderId = guestId, // if GuestId is int, convert to string
            //    Message = $"New booking for '{property.Title}' from {booking.StartDate:yyyy-MM-dd} to {booking.EndDate:yyyy-MM-dd}.",
            //};

            //await unitOfWork.NotificationRepo.AddAsync(notification);
            //await unitOfWork.SaveChanges();
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
                    TotalGuests = booking.TotalGuests,
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
                TotalGuests = booking.TotalGuests,
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

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByGuestAsync(int guestId)
        {
            var booikng = await unitOfWork.BookingRepo.GetByGuestIdAsync(guestId);
            var result = new List<BookingResponseDto>();

            foreach (var booking in booikng)
            {
                result.Add(new BookingResponseDto()
                {
                    Id = booking.Id,
                    GuestName = booking.Guest.FirstName + " " + booking.Guest.LastName,
                    TotalGuests = booking.TotalGuests,
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

        public async Task<IEnumerable<BookingResponseDto>> GetBookingsByPropertyAsync(int propertyId)
        {
            var booikng = await unitOfWork.BookingRepo.GetByPropertyIdAsync(propertyId);
            var result = new List<BookingResponseDto>();

            foreach (var booking in booikng)
            {
                result.Add(new BookingResponseDto()
                {
                    Id = booking.Id,
                    GuestName = booking.Guest.FirstName + " " + booking.Guest.LastName,
                    TotalGuests = booking.TotalGuests,
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

        public async Task<BookingStatsDto> GetBookingStatsAsync()
        {
            var booking = await unitOfWork.BookingRepo.GetAllAsync();
            return new BookingStatsDto
            {
                TotalBookings = booking.Count(),
                PendingBookings = booking.Count(b => b.Status.Equals(BookingStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase)),
                ConfirmedBookings = booking.Count(b => b.Status.Equals(BookingStatus.Confirmed.ToString(), StringComparison.OrdinalIgnoreCase)),
                CancelledBookings = booking.Count(b => b.Status.Equals(BookingStatus.Cancelled.ToString(), StringComparison.OrdinalIgnoreCase)),
                CompletedBookings = booking.Count(b => b.Status.Equals(BookingStatus.Completed.ToString(), StringComparison.OrdinalIgnoreCase)),
                DeniedBookings = booking.Count(b => b.Status.Equals(BookingStatus.Denied.ToString(), StringComparison.OrdinalIgnoreCase))
            };
        }

        public Task<Booking> GetGuestByBookingIdAsync(int bookingId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookingResponseDto>> SearchBookingsAsync(BookingSearchDto searchDto)
        {
            var bookings = await unitOfWork.BookingRepo.GetSearchBookingAsync(
        searchDto.Status,
        searchDto.FromDate,
        searchDto.ToDate,
        searchDto.GuestId,
        searchDto.PropertyId);

            var result = bookings.Select(b => new BookingResponseDto
            {
                Id = b.Id,
                PropertyTitle = b.Property?.Title ?? "N/A",
                PropertyAddress = b.Property?.Address ?? "N/A",
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                GuestName = b.Guest?.UserName ?? "N/A",
                Status = Enum.TryParse<BookingStatus>(b.Status, out var parsedStatus) ? parsedStatus : BookingStatus.Pending,
                CheckInStatus = b.CheckInStatus,
                CheckOutStatus = b.CheckOutStatus,
                TotalAmount = b.TotalAmount,
                PromotionId = b.PromotionId == 0 ? null : b.PromotionId,
                CreatedAt = b.CreatedAt
            });

            return result;

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

        public async Task<int> UpdateBookingCheckInStatusAsync(int bookingId, BookingCheckInStatusUpdate bookingCheckInStatusUpdate)
        {
            var booking = await unitOfWork.BookingRepo.GetByIdAsync(bookingId);
            if (booking == null)
                return 0;
            booking.CheckInStatus = bookingCheckInStatusUpdate.CheckInStatus;
            booking.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.BookingRepo.UpdateAsync(booking);
            await unitOfWork.SaveChanges();
            return booking.Id;
        }

        public async Task<int> UpdateBookingCheckOutStatusAsync(int bookingId,BookingCheckOutStatusUpdate bookingCheckOutStatusUpdate)
        {
            var booking = await unitOfWork.BookingRepo.GetByIdAsync(bookingId);
            if (booking == null)
                return 0;
            booking.CheckOutStatus = bookingCheckOutStatusUpdate.CheckOutStatus;
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
