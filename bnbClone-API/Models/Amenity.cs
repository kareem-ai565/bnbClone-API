using bnbClone_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace bnbClone_API.Models
{

    [Table("Amenities")]
    public class Amenity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Column("category")]
        public string? Category { get; set; }
        [Required]
        [Column("icon_url")]
        public string? IconUrl { get; set; }

        [JsonIgnore]
        public ICollection<PropertyAmenity> PropertyAmenities { get; set; } = new List<PropertyAmenity>();
    }

}
