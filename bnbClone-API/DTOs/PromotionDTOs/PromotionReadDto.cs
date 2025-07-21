namespace bnbClone_API.DTOs.PromotionDTOs

{
    public class PromotionReadDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DiscountType { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUses { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
