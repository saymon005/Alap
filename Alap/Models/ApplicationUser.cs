using Microsoft.AspNetCore.Identity;

namespace Alap.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ChatMessage> Messages { get; set; }
    }
}
