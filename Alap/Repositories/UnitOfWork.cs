using Alap.Data;
using Alap.Repositories;
using System;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IChatMessageRepository ChatMessages { get; }

    public UnitOfWork(ApplicationDbContext context, IChatMessageRepository chatRepo)
    {
        _context = context;
        ChatMessages = chatRepo;
    }

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
