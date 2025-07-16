using bnbClone_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace bnbClone_API.Models
{
    public class PropertyCategory
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }

        // Relationships
        [JsonIgnore]
        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
