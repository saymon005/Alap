using Alap.Data;
using Alap.Models;
using Alap.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

public class ChatMessageRepository : IChatMessageRepository
{
    private readonly ApplicationDbContext _context;

    public ChatMessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ChatMessage message)
    {
        await _context.ChatMessages.AddAsync(message);
    }

    public async Task<ChatMessage?> GetByIdAsync(int id)
    {
        return await _context.ChatMessages
                             .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);
    }


    public async Task<List<ChatMessage>> GetUserMessagesAsync(string userId, int page, int pageSize)
    {
        return await _context.ChatMessages
            .Where(m => m.UserId == userId && !m.IsDeleted)
            .OrderByDescending(m => m.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}

