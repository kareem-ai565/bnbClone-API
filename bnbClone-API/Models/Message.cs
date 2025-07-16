using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using bnbClone_API.Models;

namespace bnbClone_API.Models
{
    public class Message
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; } = null!;

        public int SenderId { get; set; }
        public virtual ApplicationUser Sender { get; set; } = null!;

        public int ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; } = null!;

        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public DateTime? ReadAt { get; set; }
    }
}
