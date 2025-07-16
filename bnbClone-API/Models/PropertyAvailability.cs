using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bnbClone_API.Models
{
    [Index(nameof(PropertyId), nameof(Date), IsUnique = true)]
    public class PropertyAvailability
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PropertyId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; } = true;
        public string BlockedReason { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public int MinNights { get; set; } = 1;

        // Navigation Property
        [ForeignKey("PropertyId")]
        public virtual Property Property { get; set; } = null!;
    }
}
