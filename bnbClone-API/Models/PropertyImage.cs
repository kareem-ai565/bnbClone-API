using System;
using System.Collections.Generic;
using System.Text.Json.Serialization; 
using bnbClone_API.Models;

namespace bnbClone_API.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? Description { get; set; }

        public bool IsPrimary { get; set; } = false;
        public string? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Property Property { get; set; } = null!;


    }

}
