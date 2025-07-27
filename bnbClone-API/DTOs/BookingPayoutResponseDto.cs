namespace bnbClone_API.DTOs
{
    public class BookingPayoutResponseDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        public int BookingId { get; set; }
        public string PropertyTitle { get; set; }
        public string GuestFullName { get; set; }
        public string HostFullName { get; set; }


    }
}
