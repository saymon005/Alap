namespace Alap.Repositories
{
    public interface IUnitOfWork
    {
        IChatMessageRepository ChatMessages { get; }
        Task<int> SaveChangesAsync();
    }

}
