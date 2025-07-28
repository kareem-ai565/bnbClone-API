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

        /* [HttpPost("create-intent")]
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
         }*/


        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] PaymentCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var sessionUrl = await _bookingPaymentService.CreateCheckoutSessionAsync(dto);
                return Ok(new { url = sessionUrl });
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




            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            // Get the Stripe-Signature header
            var stripeSignature = Request.Headers["Stripe-Signature"];

            try
            {
                await _bookingPaymentService.HandleStripeWebhookAsync(json, stripeSignature);
                return Ok(); // Always respond 200 to Stripe if successfully processed
            }
            catch (Exception ex)
            {
                // Optional: Log the error here
                Console.WriteLine("Webhook error: " + ex.Message);
                return BadRequest();
            }


            /*using var reader = new StreamReader(HttpContext.Request.Body);
            var json = await reader.ReadToEndAsync();

            if (!Request.Headers.TryGetValue("Stripe-Signature", out var stripeSignature))
            {
                return BadRequest(new { error = "Missing Stripe-Signature header" });
            }

            try
            {
                await _bookingPaymentService.HandleStripeWebhookAsync(json, stripeSignature);
                return Ok(); // Stripe requires 200 OK
            }
            catch (Exception ex)
            {
                // Log the exception here if needed
                return BadRequest(new { error = ex.Message });
            }*/
        }
    }
}

