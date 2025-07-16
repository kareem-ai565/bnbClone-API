using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using bnbClone_API.Models;

namespace bnbClone_API.Models
{
    public class PropertyAmenity
    {
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; } = null!;

        public int AmenityId { get; set; }
        public virtual Amenity Amenity { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

}
