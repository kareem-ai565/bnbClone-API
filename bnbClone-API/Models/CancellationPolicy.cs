using System;
using System.Collections.Generic;
using System.Text.Json.Serialization; 
using bnbClone_API.Models;

namespace bnbClone_API.Models
{
   
    public class CancellationPolicy
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal RefundPercentage { get; set; }

        public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
    }

}
