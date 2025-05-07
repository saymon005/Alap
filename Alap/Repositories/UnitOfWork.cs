using Alap.Data;
using Alap.Repositories;
using System;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IChatMessageRepository ChatMessages { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        ChatMessages = new ChatMessageRepository(_context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
