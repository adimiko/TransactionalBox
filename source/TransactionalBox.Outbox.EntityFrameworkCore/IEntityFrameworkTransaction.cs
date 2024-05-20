namespace TransactionalBox
{
    public interface IEntityFrameworkTransaction
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}
