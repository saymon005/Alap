using Alap.Models;

namespace Alap.Repositories
{
    public interface IChatMessageRepository
    {
        Task AddAsync(ChatMessage message);
        Task<ChatMessage?> GetByIdAsync(int id);
        Task<List<ChatMessage>> GetUserMessagesAsync(string userId, int page, int pageSize);
    }
}
