using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Stripe;
using Stripe.Events;
using Stripe.Checkout;



                    


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
        /*  public async Task<PaymentResponseDto> CreatePaymentIntentAsync(PaymentCreateDto paymentCreateDto)
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
  */

        /* public async Task<string> CreateCheckoutSessionAsync(PaymentCreateDto dto)
         {
             var booking = await _unitOfWork.BookingRepo.GetByIdAsync(dto.BookingId);
             if (booking == null)
                 throw new ArgumentException("Booking not found");

             var property = await _unitOfWork.PropertyRepo.GetByIdAsync(booking.PropertyId);
             if (property == null)
                 throw new ArgumentException("Property not found");

             // 1. Create Stripe Checkout Session
             var options = new Stripe.Checkout.SessionCreateOptions
             {
                 PaymentMethodTypes = new List<string> { dto.PaymentMethodType ?? "card" },
                 LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
         {
             new Stripe.Checkout.SessionLineItemOptions
             {
                 Quantity = 1,
                 PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                 {
                     Currency = "usd",
                     UnitAmount = (long)(dto.Amount * 100), // Stripe uses cents
                     ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                     {
                         Name = property.Title
                     }
                 }
             }
         },
                 Mode = "payment",
                 SuccessUrl = $"https://your-client-url.com/payment-success?bookingId={dto.BookingId}",
                 CancelUrl = "https://your-client-url.com/payment-cancelled"
             };

             var service = new Stripe.Checkout.SessionService();
             var session = await service.CreateAsync(options);

             // 2. Save payment record in your DB
             var payment = new BookingPayment
             {
                 BookingId = dto.BookingId,
                 Amount = dto.Amount,
                 PaymentMethodType = dto.PaymentMethodType ?? "card",
                 Status = "pending", // Will be updated via webhook on success
                 TransactionId = session.Id, // Store Checkout Session ID
                 PayementGateWayResponse = session.ToJson()
             };

             await _unitOfWork.BookingPaymentRepo.AddAsync(payment);
             await _unitOfWork.SaveAsync();

             // 3. Return Checkout URL for redirection
             return session.Url;
         }*/


        public async Task<string> CreateCheckoutSessionAsync(PaymentCreateDto dto)
        {
            var booking = await _unitOfWork.BookingRepo.GetByIdAsync(dto.BookingId);
            if (booking == null)
                throw new ArgumentException("Booking not found");

            var property = await _unitOfWork.PropertyRepo.GetByIdAsync(booking.PropertyId);
            if (property == null)
                throw new ArgumentException("Property not found");

            // 1. Create Stripe Checkout Session
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        {
            new Stripe.Checkout.SessionLineItemOptions
            {
                Quantity = 1,
                PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = (long)(dto.Amount * 100), // cents
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = property.Title
                    }
                }
            }
        },
                Mode = "payment",
                SuccessUrl = $"https://your-client-url.com/payment-success?bookingId={dto.BookingId}",
                CancelUrl = "https://your-client-url.com/payment-cancelled"
            };

            var sessionService = new Stripe.Checkout.SessionService();
            var session = await sessionService.CreateAsync(options);

            // ✅ 2. Save the Stripe Checkout Session Id (not the PaymentIntent)
            var payment = new BookingPayment
            {
                BookingId = dto.BookingId,
                Amount = dto.Amount,
                PaymentMethodType = "card",
                Status = "pending",
                TransactionId = session.Id, // ✅ Correct: Use session.Id
                PayementGateWayResponse = session.ToJson()
            };

            await _unitOfWork.BookingPaymentRepo.AddAsync(payment);
            await _unitOfWork.SaveAsync();

            return session.Url;
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

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    {
                        var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        if (session == null || session.PaymentIntent == null)
                            return;

                        var paymentIntentId = session.PaymentIntent.Id;
                        if (string.IsNullOrEmpty(paymentIntentId))
                            return;

                        var payment = await _unitOfWork.BookingPaymentRepo.GetByTransactionIdAsync(paymentIntentId);
                        if (payment == null)
                            return;

                        payment.Status = "Completed";
                        payment.UpdatedAt = DateTime.UtcNow;
                        await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                        await _unitOfWork.SaveAsync();

                        var booking = payment.Booking;
                        if (booking == null)
                        {
                            Console.WriteLine("Booking is NULL. Cannot create payout.");
                            return;
                        }

                        var payout = new BookingPayout
                        {
                            BookingId = booking.Id,
                            Amount = payment.Amount,
                            Status = "Pending",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await _unitOfWork.BookingPayoutRepo.AddAsync(payout);
                        await _unitOfWork.SaveAsync();

                        break;
                    }

                case "checkout.session.expired":
                case "payment_intent.payment_failed":
                    {
                        var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        if (session == null || session.PaymentIntent == null)
                            return;

                        var sessionId = session.Id;
                        if (string.IsNullOrEmpty(sessionId))
                            return;

                        var payment = await _unitOfWork.BookingPaymentRepo.GetByTransactionIdAsync(sessionId);
                        if (payment == null)
                            return;

                        payment.Status = "Failed"; // or "Refunded" or "Expired"
                        payment.UpdatedAt = DateTime.UtcNow;

                        await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                        await _unitOfWork.SaveAsync();

                        break;
                    }

                default:
                    Console.WriteLine($"Unhandled Stripe event type: {stripeEvent.Type}");
                    break;
            }
        }








        /* public async Task HandleStripeWebhookAsync(string json, string stripeSignature)
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

             switch (stripeEvent.Type)
             {
                 //case Events.CheckoutSessionCompleted:
                 case "checkout.session.completed":



                     // No other changes are needed as the error is caused by the missing namespace import.
                     {
                         var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                         if (session == null) return;

                         var paymentIntentId = session.PaymentIntent.Id ;

                         if (paymentIntentId == null || string.IsNullOrEmpty(paymentIntentId)) return;
                         var payment = await _unitOfWork.BookingPaymentRepo.GetByPaymentIntentIdAsync(paymentIntentId);

                         if (payment != null)
                         {
                             payment.Status = "Completed";
                             payment.UpdatedAt = DateTime.UtcNow;
                             await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                             await _unitOfWork.SaveAsync();

                             var booking = payment.Booking;
                             if (booking == null)
                             {
                                 Console.WriteLine("Booking is NULL. Cannot create payout.");
                                 return;
                             }

                             var payout = new BookingPayout
                             {
                                 BookingId = booking.Id,
                                 Amount = payment.Amount,
                                 Status = "Pending"
                             };

                             await _unitOfWork.BookingPayoutRepo.AddAsync(payout);
                             await _unitOfWork.SaveAsync();

                     }

                         break;
                     }

                 //case Events.CheckoutSessionExpired:
                 case "checkout.session.expired":

                 //case Events.PaymentIntentPaymentFailed:
                 case "payment_intent.payment_failed":

                     {
                         var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                         if (session == null) return;

                         var paymentIntentId = session.PaymentIntent.Id;

                         if (string.IsNullOrEmpty(paymentIntentId)) return;

                         var payment = await _unitOfWork.BookingPaymentRepo.GetByPaymentIntentIdAsync(paymentIntentId);

                         if (payment != null)
                         {
                             payment.Status = "Refunded"; // or "Pending"
                             payment.UpdatedAt = DateTime.UtcNow;

                             await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                             await _unitOfWork.SaveAsync();
                         }

                         break;
                     }

                 default:
                     break;
             }
         }
 */



        public Task<PaymentResponseDto> CreatePaymentIntentAsync(PaymentCreateDto paymentCreateDto)
        {
            throw new NotImplementedException();
        }

        /*public async Task HandleStripeWebhookAsync(string json, string stripeSignature)
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
        }*/
    }
}
