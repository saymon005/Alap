using System.ComponentModel.DataAnnotations;

namespace Alap.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        public string? SessionId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Sender name cannot exceed 50 characters.")]
        public string Sender { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters.")]
        public string Message { get; set; }

        public bool IsDeleted { get; set; } = false;
        public bool IsApproved { get; set; } = false;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
