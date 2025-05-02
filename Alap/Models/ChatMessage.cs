namespace Alap.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? SessionId { get; set; }
        public string Sender { get; set; }  // "User" or "Bot"
        public string Message { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

       // public ApplicationUser User { get; set; }
    }
}
