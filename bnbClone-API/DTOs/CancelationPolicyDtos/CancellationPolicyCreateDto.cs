namespace bnbClone_API.DTOs.CancelationPolcyDts
{
    public class CancellationPolicyCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal RefundPercentage { get; set; }
    }
}
