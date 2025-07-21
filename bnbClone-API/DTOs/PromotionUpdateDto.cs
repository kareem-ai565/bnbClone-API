using System.ComponentModel.DataAnnotations;

namespace bnbClone_API.DTOs
{
    public class PromotionUpdateDto
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(20)]
        public string DiscountType { get; set; }

        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MaxUses { get; set; }
        public bool IsActive { get; set; }
    }

}
