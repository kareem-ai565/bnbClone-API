namespace bnbClone_API.DTOs
{
    public class CancellationPolicyUpdateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal RefundPercentage { get; set; }
    }
}
