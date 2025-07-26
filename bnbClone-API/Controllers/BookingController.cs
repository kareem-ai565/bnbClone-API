using bnbClone_API.DTOs;
using bnbClone_API.Services.Impelementations;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
        [HttpPost("CreateBookingByUserId{id:int}")]
        public async Task<IActionResult> CreateBooking(int id,[FromBody] BookingCreateDto bookingCreateDto)
        {
            if (bookingCreateDto == null)
            {
                return BadRequest("Booking data is required.");
            }
            var bookingid = await _bookingService.AddBooking(id,bookingCreateDto);
            if (bookingid > 0)
            {
                return CreatedAtAction(nameof(GetBookings), new { id = bookingid }, bookingid);
            }
            return BadRequest("Failed to create booking.");
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound($"Booking with ID {id} not found.");
            }
            return Ok(booking);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBooking(int id, [FromBody] BookingUpdateDto bookingUpdateDto)
        {
           if(bookingUpdateDto == null)
            {
                return BadRequest("Booking data is required");
            }
           var result = await _bookingService.UpdateBooking(id, bookingUpdateDto);
            if(result > 0)
            {
                return Ok($"Booking with ID {id} Updated successfully.");
            }
            return NotFound($"Booking with ID {id} not found.");
        }

        [HttpPut("Update-Status/{id:int}")]

        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] BookingStatusUpdateDto bookingStatusUpdateDto)
        {
            if (bookingStatusUpdateDto == null)
            {
                return BadRequest("Booking status data is required.");
            }
            var result = await _bookingService.UpdateBookingStatusAsync(id, bookingStatusUpdateDto);
            if (result > 0)
            {
                return Ok($"Booking with ID {id} status updated successfully.");
            }
            return NotFound($"Booking with ID {id} not found.");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var result = await _bookingService.DeleteBooking(id);
            if (result > 0)
            {
                return Ok($"Booking with ID {id} deleted successfully.");
            }
            return NotFound($"Booking with ID {id} not found.");
        }

        [HttpGet("ByGuestId/{guestId:int}")]
        public async Task<IActionResult> GetBookingsByGuest(int guestId)
        {
            var bookings = await _bookingService.GetBookingsByGuestAsync(guestId);
            if (bookings == null || !bookings.Any())
            {
                return NotFound($"No bookings found for guest with ID {guestId}.");
            }
            return Ok(bookings);
        }
        [HttpGet("ByPropertyId/{propertyId:int}")]
        public async Task<IActionResult> GetBookingsByProperty(int propertyId)
        {
            var bookings = await _bookingService.GetBookingsByPropertyAsync(propertyId);
            if (bookings == null || !bookings.Any())
            {
                return NotFound($"No bookings found for property with ID {propertyId}.");
            }
            return Ok(bookings);
        }
        [HttpPost("Search")]
        public async Task<IActionResult> SearchBookings([FromBody] BookingSearchDto searchDto)
        {
            if (searchDto == null)
            {
                return BadRequest("Search criteria is required.");
            }
            var bookings = await _bookingService.SearchBookingsAsync(searchDto);
            if (bookings == null || !bookings.Any())
            {
                return NotFound("No bookings found matching the search criteria.");
            }
            return Ok(bookings);
        }

        [HttpGet("Stats")]
        public async Task<IActionResult> GetBookingStats()
        {
            var stats = await _bookingService.GetBookingStatsAsync();
            if (stats == null)
            {
                return NotFound("No booking statistics found.");
            }
            return Ok(stats);
        }
        [HttpPut("Update-CheckInStatus/{id:int}")]
        public async Task<IActionResult> UpdateBookingCheckInStatus(int id, [FromBody] BookingCheckInStatusUpdate bookingCheckInStatusUpdate)
        {
            if (bookingCheckInStatusUpdate == null)
            {
                return BadRequest("Booking check-in status data is required.");
            }
            var result = await _bookingService.UpdateBookingCheckInStatusAsync(id, bookingCheckInStatusUpdate);
            if (result > 0)
            {
                return Ok($"Booking with ID {id} check-in status updated successfully.");
            }
            return NotFound($"Booking with ID {id} not found.");
        }
        [HttpPut("Update-CheckOutStatus/{id:int}")]
        public async Task<IActionResult> UpdateBookingCheckOutStatus(int id, [FromBody] BookingCheckOutStatusUpdate bookingCheckOutStatusUpdate)
        {
            if (bookingCheckOutStatusUpdate == null)
            {
                return BadRequest("Booking check-out status data is required.");
            }
            var result = await _bookingService.UpdateBookingCheckOutStatusAsync(id, bookingCheckOutStatusUpdate);
            if (result > 0)
            {
                return Ok($"Booking with ID {id} check-out status updated successfully.");
            }
            return NotFound($"Booking with ID {id} not found.");
        }
        [HttpGet("host/{hostId}")]
        public async Task<ActionResult<IEnumerable<BookingResponseDto>>> GetByHostId(int hostId)
        {
            var bookings = await _bookingService.GetBookingsByHostAsync(hostId);

            if (bookings == null || !bookings.Any())
                return NotFound($"No bookings found for host ID {hostId}");

            return Ok(bookings);
        }

    }

}
