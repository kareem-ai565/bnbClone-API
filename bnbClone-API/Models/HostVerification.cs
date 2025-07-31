using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using bnbClone_API.Models;

namespace bnbClone_API.Models
{
    public class HostVerification
    {
      
            public int Id { get; set; }
            public int HostId { get; set; }
            public string Type { get; set; }=string.Empty;
            public string Status { get; set; } = "pending";
            public string DocumentUrl1 { get; set; }
            public string DocumentUrl2 { get; set; }
            public DateTime SubmittedAt { get; set; } = DateTime.Now;
            public DateTime? VerifiedAt { get; set; }
            public string AdminNotes { get; set; } = string.Empty; // Add this if not present



            public virtual Host Host { get; set; }
        

    }
}
