namespace bnbClone_API.Models
{
    public class ActivePromotionView
    {
        public int PromotionId { get; set; }
        public string Code { get; set; } = null!;
        public string DiscountType { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? MaxUses { get; set; }
        public int UsedCount { get; set; }
        public int RemainingUses { get; set; }
    }
}
