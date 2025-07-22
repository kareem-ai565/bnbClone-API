using bnbClone_API.DTOs;
using bnbClone_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingPaymentController : ControllerBase
    {
        private readonly IBookingPaymentService _bookingPaymentService;

        public BookingPaymentController(IBookingPaymentService bookingPaymentService)
        {
            _bookingPaymentService = bookingPaymentService;
        }
        
        /// Create Stripe Payment Intent
        
        [HttpPost("create-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var response = await _bookingPaymentService.CreatePaymentIntentAsync(dto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        /// Stripe webhook handler (no auth)
        
        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook()
        {
            using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();

            var stripeSignature = Request.Headers["Stripe-Signature"];

            try
            {
                await _bookingPaymentService.HandleStripeWebhookAsync(json, stripeSignature!);
                return Ok(); // Must return 200 OK to Stripe
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

