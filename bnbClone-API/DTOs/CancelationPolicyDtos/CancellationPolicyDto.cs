namespace bnbClone_API.DTOs.CancelationPolcyDts
{
    public class CancellationPolicyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal RefundPercentage { get; set; }
    }
}
