using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingPayoutController : ControllerBase
    {
        private readonly IBookingPayoutService _bookingPayoutService;

        public BookingPayoutController(IBookingPayoutService bookingPayoutService)
        {
            _bookingPayoutService = bookingPayoutService;
        }

        [HttpGet("AdmenGetAllPayout")]
        public async Task<IActionResult> GetPayouts()
        {
            var payouts = await _bookingPayoutService.GetAllPayoutsToHostAsync();
            return Ok(payouts);
        }
        //  Admin: Manually trigger payout release for a booking
        [HttpPost("AdmenReleasePayout/{bookingId}")]
        public async Task<IActionResult> ReleasePayout(int bookingId)
        {
            var success = await _bookingPayoutService.ReleasePayoutToHostAsync(bookingId);
            if (!success)
                return BadRequest("Payout cannot be released. Make sure the check-in is completed and payout is pending.");

            return Ok("Payout released successfully.");
        }
    }
}
