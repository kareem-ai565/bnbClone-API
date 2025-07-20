namespace bnbClone_API.DTOs
{
    public class CancellationPolicyCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal RefundPercentage { get; set; }
    }
}
