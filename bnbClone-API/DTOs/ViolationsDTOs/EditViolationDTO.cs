using bnbClone_API.Models;

namespace bnbClone_API.DTOs.ViolationsDTOs
{
    public class EditViolationDTO
    {
        public ViolationType ViolationType { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = ViolationStatus.Pending.ToString();
        public string? AdminNotes { get; set; }
    }

}
