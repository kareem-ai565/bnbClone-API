namespace bnbClone_API.Models
{
    public class UserBookingHistoryView
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public string GuestName { get; set; } = null!;
        public string PropertyTitle { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string BookingStatus { get; set; } = null!;
        public decimal? Amount { get; set; }
        public string? PaymentMethodType { get; set; }
        public string? PaymentStatus { get; set; }
    }
}
