namespace bnbClone_API.DTOs
{
    public class PaymentCreateDto
    {
       /* public int BookingId { get; set; }
        public BookingCreateDto Booking { get; set; }
        public decimal Amount { get; set; }*/
        //public string PaymentMethodType { get; set; } = "card";

        public int PropertyId { get; set; }
        public int GuestId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalGuests { get; set; }
        public int? PromotionId { get; set; }
        public decimal Amount { get; set; }
    }
}
