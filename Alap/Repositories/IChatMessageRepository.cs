using Alap.Models;

namespace Alap.Repositories
{
    public interface IChatMessageRepository
    {
        Task<IEnumerable<ChatMessage>> GetUserMessagesAsync(string userId, int page, int pageSize);
        Task<ChatMessage> GetByIdAsync(int id);
        Task AddAsync(ChatMessage message);
        void Update(ChatMessage message);
    }
}
