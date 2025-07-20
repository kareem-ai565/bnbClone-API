using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Stripe;
using Stripe.Events; // Add this import to resolve the 'Events' namespace issue





namespace bnbClone_API.Services.Impelementations
{
    public class BookingPaymentService : IBookingPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public BookingPaymentService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<PaymentResponseDto> CreatePaymentIntentAsync(PaymentCreateDto paymentCreateDto)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(paymentCreateDto.BookingId);
            if (booking == null)
                throw new Exception("Booking not found");

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(paymentCreateDto.Amount * 100), // in cents
                Currency = "usd",
                PaymentMethodTypes = new List<string> { paymentCreateDto.PaymentMethodType }
            };

            var service = new PaymentIntentService();
            var intent = await service.CreateAsync(options);

            var payment = new BookingPayment
            {
                BookingId = paymentCreateDto.BookingId,
                Amount = paymentCreateDto.Amount,
                PaymentMethodType = paymentCreateDto.PaymentMethodType,
                Status = "pending",
                TransactionId = intent.Id,
                PayementGateWayResponse = intent.ToJson()
            };

            await _unitOfWork.BookingPaymentRepo.AddAsync(payment);
            await _unitOfWork.SaveAsync();

            return new PaymentResponseDto
            {
                ClientSecret = intent.ClientSecret,
                PaymentIntentId = intent.Id
            };
        }

        public async Task HandleStripeWebhookAsync(string json, string stripeSignature)
        {
            var endpointSecret = _configuration["Stripe:WebhookSecret"];
            Event stripeEvent;

            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, endpointSecret);
            }
            catch (StripeException)
            {
                throw new Exception("Invalid webhook signature.");
            }

            
            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;
                var payment = await _unitOfWork.BookingPaymentRepo.GetByPaymentIntentIdAsync(intent.Id);
                if (payment != null)
                {
                    payment.Status = "succeeded";
                    payment.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                    // Optionally: Create payout record
                    var booking = payment.Booking;
                    var payout = new BookingPayout
                    {
                        BookingId = booking.Id,
                        Amount = payment.Amount,
                        Status = "Pending"
                    };
                    await _unitOfWork.BookingPayoutRepo.AddAsync(payout);
                    await _unitOfWork.SaveAsync();
                }
            }
            else if (stripeEvent.Type == EventTypes.PaymentIntentPaymentFailed)
            {
                var intent = (PaymentIntent)stripeEvent.Data.Object;
                var payment = await _unitOfWork.BookingPaymentRepo.GetByPaymentIntentIdAsync(intent.Id);
                if (payment != null)
                {
                    payment.Status = "failed";
                    payment.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                    await _unitOfWork.SaveAsync();
                }
            }
        }
    }
}
