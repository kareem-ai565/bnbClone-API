namespace bnbClone_API.Models
{
    public class Notification
    {
            public int Id { get; set; }

            public int UserId { get; set; }

            public int? SenderId { get; set; }

            public string Message { get; set; }
            public bool IsRead { get; set; } = false;

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public virtual ApplicationUser User { get; set; }

            public virtual ApplicationUser Sender { get; set; }
        
    }
}
