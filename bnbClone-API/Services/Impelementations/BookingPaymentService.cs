using bnbClone_API.DTOs;
using bnbClone_API.Models;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;
using Stripe;
using Stripe.Events;
using Stripe.Checkout;
using System.Text.Json;






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


        //public async Task<string> CreateCheckoutSessionAsync(PaymentCreateDto dto)
        //{
        //    /*  var booking = await _unitOfWork.BookingRepo.GetByIdAsync(dto.BookingId);
        //      if (booking == null)
        //          throw new ArgumentException("Booking not found");

        //      var property = await _unitOfWork.PropertyRepo.GetByIdAsync(booking.PropertyId);
        //      if (property == null)
        //          throw new ArgumentException("Property not found");
        //      */
        //    var bookingcreateDto = dto.Booking;


        //    var property = await _unitOfWork.PropertyRepo.GetByIdAsync(bookingcreateDto.PropertyId); // dto.BookingId is misused, should be fixed
        //    if (property == null)
        //        throw new ArgumentException("Property not found");

        //    // FOR THIS TO WORK — you must actually pass BookingCreateDto as part of the request
        //    // This example assumes you receive it as part of your frontend logic

        //    // 1. Create Stripe Checkout Session
        //    var options = new Stripe.Checkout.SessionCreateOptions
        //    {
        //        PaymentMethodTypes = new List<string> { "card" },
        //        LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
        //{
        //    new Stripe.Checkout.SessionLineItemOptions
        //    {
        //        Quantity = 1,
        //        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
        //        {
        //            Currency = "usd",
        //            UnitAmount = (long)(dto.Amount * 100), // cents
        //            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
        //            {
        //                Name = property.Title
        //            }
        //        }
        //    }
        //},
        //        Mode = "payment",
        //        SuccessUrl = $"https://your-client-url.com/payment-success?bookingId",
        //        CancelUrl = "https://your-client-url.com/payment-cancelled",
        //        Metadata = new Dictionary<string, string>
        //        {

        //            { "propertyId", bookingcreateDto.PropertyId.ToString("1") },
        //            { "startDate", bookingcreateDto.StartDate.ToString("2025-11-10T09:28:26.587Z") },
        //            { "endDate", bookingcreateDto.EndDate.ToString("2025-11-12T06:28:26.587Z") },
        //            { "totalGuests", bookingcreateDto.TotalGuests.ToString("2") },
        //            { "promotionId", bookingcreateDto.PromotionId?.ToString() ?? "0" },
        //            { "amount", dto.Amount.ToString("450") },
        //            { "userId", "1" } // pass actual user ID from your context
        //        }
        //    };


        //    var sessionService = new Stripe.Checkout.SessionService();
        //    var session = await sessionService.CreateAsync(options);

        //   /* // ✅ 2. Save the Stripe Checkout Session Id (not the PaymentIntent)
        //    var payment = new BookingPayment
        //    {
        //        BookingId = dto.BookingId,
        //        Amount = dto.Amount,
        //        PaymentMethodType = "card",
        //        Status = "pending",
        //        TransactionId = session.Id, // ✅ Correct: Use session.Id
        //        PayementGateWayResponse = session.ToJson()
        //    };

        //    await _unitOfWork.BookingPaymentRepo.AddAsync(payment);
        //    await _unitOfWork.SaveAsync();*/


        //    return session.Url;
        //}


        /*   public async Task<string> CreateCheckoutSessionAsync(PaymentCreateDto dto)
           {
               var booking = await _unitOfWork.BookingRepo.GetByIdAsync(dto.);
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
   */
        /*-*-*-*-*-*-
                public async Task<string> CreateCheckoutSessionAsync(PaymentCreateDto dto)
                {
                    var property = await _unitOfWork.PropertyRepo.GetByIdAsync(dto.PropertyId);
                    if (property == null)
                        throw new ArgumentException("Property not found");

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
                            UnitAmount = (long)(dto.Amount * 100),
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = $"Booking: {property.Title}"
                            }
                        }
                    }
                },


                        Mode = "payment",
                        SuccessUrl = $"https://your-client-url.com/payment-success",
                        CancelUrl = "https://your-client-url.com/payment-cancelled",
                        Metadata = new Dictionary<string, string>
                {
                    { "propertyId", dto.PropertyId.ToString() },
                    { "userId", dto.GuestId.ToString() },
                    { "startDate", dto.StartDate.ToString("yyyy-MM-dd") },
                    { "endDate", dto.EndDate.ToString("yyyy-MM-dd") },
                    { "guestCount", dto.TotalGuests.ToString() },
                    { "amount", dto.Amount.ToString() },
                    { "promotionId", dto.PromotionId?.ToString() ?? "0" }
                }
                    };
                    // ✅ This adds metadata to the underlying PaymentIntent (required for webhook handling)
                    new Stripe.Checkout.SessionPaymentIntentDataOptions
                    {
                        Metadata = new Dictionary<string, string>
                    {
                        { "propertyId", dto.PropertyId.ToString() },
                        { "userId", dto.GuestId.ToString() },
                        { "startDate", dto.StartDate.ToString("yyyy-MM-dd") },
                        { "endDate", dto.EndDate.ToString("yyyy-MM-dd") },
                        { "guestCount", dto.TotalGuests.ToString() },
                        { "amount", dto.Amount.ToString() },
                        { "promotionId", dto.PromotionId?.ToString() ?? "0" }
                    }
                    };
                    var sessionService = new Stripe.Checkout.SessionService();
                    var session = await sessionService.CreateAsync(options);

                    return session.Url;
                }*/


        public async Task<string> CreateCheckoutSessionAsync(PaymentCreateDto dto)
        {
            var property = await _unitOfWork.PropertyRepo.GetByIdAsync(dto.PropertyId);
            if (property == null)
                throw new ArgumentException("Property not found");

            var metadata = new Dictionary<string, string>
    {
        { "propertyId", dto.PropertyId.ToString() },
        { "userId", dto.GuestId.ToString() },
        { "startDate", dto.StartDate.ToString("yyyy-MM-dd") },
        { "endDate", dto.EndDate.ToString("yyyy-MM-dd") },
        { "guestCount", dto.TotalGuests.ToString() },
        { "amount", dto.Amount.ToString() },
        { "promotionId", dto.PromotionId?.ToString() ?? "0" }
    };

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
                    UnitAmount = (long)(dto.Amount * 100),
                    ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Booking: {property.Title}"
                    }
                }
            }
        },
                Mode = "payment",
                SuccessUrl = "https://your-client-url.com/payment-success",
                CancelUrl = "https://your-client-url.com/payment-cancelled",

                // ✅ metadata for the session (optional)
                Metadata = metadata,

                // ✅ this is what you missed: attach to PaymentIntent
                PaymentIntentData = new Stripe.Checkout.SessionPaymentIntentDataOptions
                {
                    Metadata = metadata
                }
            };

            var sessionService = new Stripe.Checkout.SessionService();
            var session = await sessionService.CreateAsync(options);

            return session.Url;
        }



        public async Task HandleStripeWebhookAsync(string json, string stripeSignature)
        {
            const string paymentSuccessStatus = "paid";
            var endpointSecret = _configuration["Stripe:WebhookSecret"];

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, endpointSecret);
                Dictionary<string, string> metadata = null;
                string transactionId = null;
                string paymentIntentId = null;
                string paymentStatus = null;
                decimal amount = 0;
                string paymentMethod = "card";
                string eventType = stripeEvent.Type;
                string PayementGateWayResponse = null;

                Console.WriteLine($"Received Stripe event: {eventType}");

                // Handle all possible successful payment events
                if (eventType == EventTypes.CheckoutSessionCompleted ||
                    eventType == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (session == null) return;

                    var sessionService = new SessionService();
                    var completeSession = await sessionService.GetAsync(session.Id);

                    metadata = completeSession.Metadata;
                    transactionId = completeSession.Id;
                    PayementGateWayResponse = session.ToJson();
                    paymentStatus = completeSession.PaymentStatus;
                    amount = completeSession.AmountTotal.HasValue ? completeSession.AmountTotal.Value / 100m : 0m;

                    Console.WriteLine($"Checkout Session: {completeSession.Id}, Status: {paymentStatus}");
                }
                else if (eventType == EventTypes.PaymentIntentSucceeded ||
                         eventType == "payment_intent.succeeded")
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    if (paymentIntent == null) return;

                    metadata = paymentIntent.Metadata;
                    paymentIntentId = paymentIntent.Id;
                    paymentStatus = paymentIntent.Status;
                    amount = paymentIntent.Amount / 100m;
                    paymentMethod = paymentIntent.PaymentMethod?.Type ?? "card";

                    Console.WriteLine($"PaymentIntent: {paymentIntent.Id}, Status: {paymentStatus}");
                }
                else if (eventType == EventTypes.ChargeSucceeded ||
                         eventType == "charge.succeeded")
                {
                    var charge = stripeEvent.Data.Object as Charge;
                    if (charge == null) return;

                    metadata = charge.Metadata ?? new Dictionary<string, string>();
                    paymentIntentId = charge.PaymentIntentId ?? charge.Id;
                    paymentStatus = charge.Status;
                    amount = charge.Amount / 100m;
                    paymentMethod = charge.PaymentMethodDetails?.Type ?? "card";

                    Console.WriteLine($"Charge: {charge.Id}, Status: {paymentStatus}");
                }
                else
                {
                    Console.WriteLine($"Unhandled event type: {eventType}");
                    return;
                }

                // Validate payment was successful
                if (paymentStatus != paymentSuccessStatus)
                {
                    Console.WriteLine($"Payment not completed for transaction {transactionId}. Status: {paymentStatus}");
                    return;
                }

                // Validate required metadata exists
                if (metadata == null ||
                    !metadata.ContainsKey("propertyId") ||
                    !metadata.ContainsKey("startDate") ||
                    !metadata.ContainsKey("userId"))
                {
                    Console.WriteLine($"Missing required metadata in transaction {transactionId}");
                    return;
                }

                // Parse metadata with error handling
                try
                {
                    // 1. Create and save booking
                    var booking = new Booking
                    {

                        PropertyId = int.Parse(metadata["propertyId"]),
                        GuestId = int.Parse(metadata["userId"]),
                        StartDate = DateTime.Parse(metadata["startDate"]),
                        EndDate = metadata.ContainsKey("endDate") ? DateTime.Parse(metadata["endDate"]) : DateTime.Parse(metadata["startDate"]).AddDays(1),
                        TotalGuests = metadata.ContainsKey("totalGuests") ? int.Parse(metadata["totalGuests"]) : 1,
                        TotalAmount = metadata.ContainsKey("amount") ? decimal.Parse(metadata["amount"]) : amount,
                        PromotionId = metadata.ContainsKey("promotionId") && !string.IsNullOrEmpty(metadata["promotionId"]) ?
                            int.Parse(metadata["promotionId"]) : 0,
                        CreatedAt = DateTime.UtcNow,
                        Status = BookingStatus.Pending.ToString(),
                    };

                    await _unitOfWork.BookingRepo.AddAsync(booking);
                    await _unitOfWork.SaveAsync();

                    // 2. Create payment record
                    var payment = new BookingPayment
                    {
                        BookingId = booking.Id,
                        Amount = metadata.ContainsKey("amount") ? decimal.Parse(metadata["amount"]) : amount,
                        PaymentMethodType = paymentMethod,
                        Status = paymentStatus,
                        PaymentIntentId = paymentIntentId?? "defaultIntentId",
                        TransactionId = transactionId?? "defaultTransactionId",
                        PayementGateWayResponse = PayementGateWayResponse ?? "defaultPayementGateWayResponse",                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        RefundedAmount = 0 // Assuming no refunds at this point



                        /*BookingId = booking.Id,
                        Amount = amount,
                        PaymentMethodType = "card",
                        Status = "pending",
                        PaymentIntentId = paymentIntentId?? "defaultIntentId",
                        TransactionId =transactionId?? "defaultTransactionId", // ✅ Correct: Use session.Id
                        PayementGateWayResponse = PayementGateWayResponse?? "defaultPayementGateWayResponse"*/

                    };

                    await _unitOfWork.BookingPaymentRepo.AddAsync(payment);
                    await _unitOfWork.SaveAsync();

                    // 3. Create payout record
                    var payout = new BookingPayout
                    {
                        BookingId = booking.Id,
                        Amount = payment.Amount,
                        Status = "pending",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.BookingPayoutRepo.AddAsync(payout);
                    await _unitOfWork.SaveAsync();

                    // 4. Notify host


                    var property = await _unitOfWork.PropertyRepo.GetByIdAsync(booking.PropertyId);
                    if (property != null)
                    {


                        // Host Notification
                        var hostId = property.HostId;
                        var guestId = booking.GuestId;

                        var notification = new Notification
                        {
                            UserId = hostId,
                            SenderId = guestId, // if GuestId is int, convert to string
                            Message = $"New booking for '{property.Title}' from {booking.StartDate:yyyy-MM-dd} to {booking.EndDate:yyyy-MM-dd}.",
                        };

                        await _unitOfWork.NotificationRepo.AddAsync(notification);
                        await _unitOfWork.SaveAsync();
                        /*  await _unitOfWork.NotificationRepo.AddAsync(new Notification
                          {
                              UserId = property.HostId,
                              SenderId = booking.GuestId,
                              Message = $"New booking for '{property.Title}' from {booking.StartDate:yyyy-MM-dd} to {booking.EndDate:yyyy-MM-dd}.",
                              CreatedAt = DateTime.UtcNow
                          });*/
                    }

                    await _unitOfWork.SaveAsync();
                    Console.WriteLine($"Successfully processed payment. Booking ID: {booking.Id}, Amount: {amount:C}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Metadata parsing error: {ex.Message}");
                    Console.WriteLine($"Problematic metadata values: {JsonSerializer.Serialize(metadata)}");
                }
            }
            catch (StripeException e)
            {
                Console.WriteLine($"Stripe error: {e.StripeError?.Message ?? e.Message}");
                Console.WriteLine($"Webhook JSON: {json}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: {e}");
                Console.WriteLine($"Stack trace: {e.StackTrace}");
            }
        }





















        //public async Task HandleStripeWebhookAsync(string json, string stripeSignature)
        //{
        //    var endpointSecret = _configuration["Stripe:WebhookSecret"];
        //    Event stripeEvent;

        //    try
        //    {
        //        stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, endpointSecret);
        //    }
        //    catch (StripeException)
        //    {
        //        throw new Exception("Invalid webhook signature.");
        //    }

        //    if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted || stripeEvent.Type == EventTypes.PaymentIntentSucceeded ||
        //        stripeEvent.Type == "checkout.session.completed")
        //    {
        //        var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
        //        if (session == null) return;

        //        var metadata = session.Metadata;

        //        // 1. Create Booking
        //        var booking = new Booking
        //        {
        //            PropertyId = int.Parse(metadata["propertyId"]),
        //            StartDate = DateTime.Parse(metadata["startDate"]),
        //            EndDate = DateTime.Parse(metadata["endDate"]),
        //            TotalGuests = int.Parse(metadata["totalGuests"]),
        //            PromotionId = string.IsNullOrEmpty(metadata["promotionId"]) ? 0 : int.Parse(metadata["promotionId"]),
        //            GuestId = int.Parse(metadata["userId"]),
        //            CreatedAt = DateTime.UtcNow,
        //            Status = "Pending"
        //        };

        //        await _unitOfWork.BookingRepo.AddAsync(booking);
        //        await _unitOfWork.SaveAsync(); // Booking.Id now available

        //        // 2. Create BookingPayment
        //        var payment = new BookingPayment
        //        {
        //            BookingId = booking.Id,
        //            Amount = decimal.Parse(metadata["amount"]),
        //            PaymentMethodType = "card",
        //            Status = session.PaymentStatus, // typically "paid"
        //            TransactionId = session.Id,
        //            PayementGateWayResponse = session.ToJson(),
        //            CreatedAt = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow
        //        };

        //        await _unitOfWork.BookingPaymentRepo.AddAsync(payment);

        //        // 3. Create BookingPayout (pending)
        //        var payout = new BookingPayout
        //        {
        //            BookingId = booking.Id,
        //            Amount = payment.Amount,
        //            Status = "pending",
        //            CreatedAt = DateTime.UtcNow,
        //            UpdatedAt = DateTime.UtcNow
        //        };

        //        await _unitOfWork.BookingPayoutRepo.AddAsync(payout);

        //        // 4. Notify Host and Admin
        //        var property = await _unitOfWork.PropertyRepo.GetByIdAsync(booking.PropertyId);
        //        if (property != null)
        //        {
        //            var hostId = property.HostId;
        //            var guestId = booking.GuestId;

        //            var hostNotification = new Notification
        //            {
        //                UserId = hostId,
        //                SenderId = guestId,
        //                Message = $"New booking for '{property.Title}' from {booking.StartDate:yyyy-MM-dd} to {booking.EndDate:yyyy-MM-dd}.",
        //                CreatedAt = DateTime.UtcNow
        //            };

        //            /* var adminNotification = new Notification
        //             {
        //                 UserId = 5, // assuming Admin user ID = 1
        //                 SenderId = guestId,
        //                 Message = $"Booking confirmed for '{property.Title}' from {booking.StartDate:yyyy-MM-dd} to {booking.EndDate:yyyy-MM-dd}.",
        //                 CreatedAt = DateTime.UtcNow
        //             };*/

        //            await _unitOfWork.NotificationRepo.AddAsync(hostNotification);
        //            //await _unitOfWork.NotificationRepo.AddAsync(adminNotification);
        //        }

        //        // Final Save
        //        await _unitOfWork.SaveAsync();
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
        //    }
        //}






        /*  public async Task HandleStripeWebhookAsync(string json, string stripeSignature)
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

              if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
              {
                  var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                  if (session == null) return;

                  var sessionId = session.Id;

                  var payment = await _unitOfWork.BookingPaymentRepo.GetByTransactionIdAsync(sessionId);
                  if (payment == null) return;

                  payment.Status = session.PaymentStatus; // usually "paid"
                  payment.UpdatedAt = DateTime.UtcNow;
                  await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                  await _unitOfWork.SaveAsync(); // <- this must be called after update

                  var booking = payment.Booking;
                  if (booking == null) return;

                  var payout = new BookingPayout
                  {
                      BookingId = booking.Id,
                      Amount = payment.Amount,
                      Status = "pending",
                      CreatedAt = DateTime.UtcNow,
                      UpdatedAt = DateTime.UtcNow
                  };

                  await _unitOfWork.BookingPayoutRepo.AddAsync(payout);
                  await _unitOfWork.SaveAsync();
              }
              else
              {
                  Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
              }
          }*/

















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

            switch (stripeEvent.Type)
            {
                case EventTypes.CheckoutSessionCompleted:
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    if (session == null) return;

                    var sessionId = session.Id;

                    var payment = await _unitOfWork.BookingPaymentRepo.GetByTransactionIdAsync(sessionId);
                    if (payment == null) return;

                    payment.Status = session.PaymentStatus;
                    payment.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);

                    var booking = payment.Booking;
                    if (booking == null) return;

                    var payout = new BookingPayout
                    {
                        BookingId = booking.Id,
                        Amount = payment.Amount,
                        Status = "pending",
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _unitOfWork.BookingPayoutRepo.AddAsync(payout);
                    await _unitOfWork.SaveAsync();
                    break;

                case EventTypes.CheckoutSessionExpired:
                case EventTypes.CheckoutSessionAsyncPaymentFailed:
                    var failedSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    if (failedSession == null) return;

                    var failedPayment = await _unitOfWork.BookingPaymentRepo.GetByTransactionIdAsync(failedSession.Id);
                    if (failedPayment == null) return;

                    failedPayment.Status = "Failed";
                    failedPayment.UpdatedAt = DateTime.UtcNow;
                    await _unitOfWork.BookingPaymentRepo.UpdateAsync(failedPayment);
                    await _unitOfWork.SaveAsync();
                    break;

                default:
                    Console.WriteLine($"Unhandled event type: {stripeEvent.Type}");
                    break;
            }

        }*/


        //--------------------------------------7777---------------------------------
        /*
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
                        case EventTypes.CheckoutSessionCompleted:
                            {
                                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                                if (session == null)
                                    return;

                                var sessionId = session.Id; // 

                                var payment = await _unitOfWork.BookingPaymentRepo.GetByTransactionIdAsync(sessionId);
                                if (payment == null)
                                    return;

                                payment.Status = "completed"; // or "Paid" if that matches your enum/string values
                                payment.UpdatedAt = DateTime.UtcNow;
                                await _unitOfWork.BookingPaymentRepo.UpdateAsync(payment);
                                await _unitOfWork.SaveAsync();

                                var booking = payment.Booking;
                                if (booking == null)
                                    return;

                                var payout = new BookingPayout
                                {
                                    BookingId = booking.Id,
                                    Amount = payment.Amount,
                                    Status = "pending",
                                    CreatedAt = DateTime.UtcNow,
                                    UpdatedAt = DateTime.UtcNow
                                };

                                await _unitOfWork.BookingPayoutRepo.AddAsync(payout);
                                await _unitOfWork.SaveAsync();

                                break;
                            }


                        case EventTypes.CheckoutSessionExpired:
                        case EventTypes.CheckoutSessionAsyncPaymentFailed:
                            {
                                var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                                if (session == null)
                                    return;

                                var sessionId = session.Id; // not session.PaymentIntent.Id
                                var payment = await _unitOfWork.BookingPaymentRepo.GetByTransactionIdAsync(sessionId);
                                if (payment == null)
                                    return;

                                payment.Status = "Failed";
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
        */


        //------------------------------------------777777------------------------------




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
