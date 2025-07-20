namespace bnbClone_API.DTOs
{
    public class BookingUpdateDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalGuests { get; set; }
        public int? PromotionId { get; set; }
    }
}
