using bnbClone_API.DTOs;

namespace bnbClone_API.Services.Interfaces
{
    public interface IBookingPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentIntentAsync(PaymentCreateDto paymentCreateDto);
        Task HandleStripeWebhookAsync(string json, string stripeSignature);
    }
}
