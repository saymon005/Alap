using System.ComponentModel.DataAnnotations;

namespace Alap.DTOs
{
    public class SendMessageRequest
    {
        [Required]
        [StringLength(500, MinimumLength = 1)]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Message cannot be empty or whitespace.")]
        public string Message { get; set; }
    }
}
