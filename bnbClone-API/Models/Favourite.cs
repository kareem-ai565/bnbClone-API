using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using bnbClone_API.Models;

namespace bnbClone_API.Models
{
    public class Favourite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PropertyId { get; set; }
        public DateTime FavouritedAt { get; set; } = DateTime.UtcNow;
      
    
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Property Property { get; set; } = null!;
    }
}
