namespace bnbClone_API.DTOs
{
    public class UserUsedPromotionDTO
    {
        public int PromotionId { get; set; }
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public decimal DiscountedAmount { get; set; }
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;

    }
}
