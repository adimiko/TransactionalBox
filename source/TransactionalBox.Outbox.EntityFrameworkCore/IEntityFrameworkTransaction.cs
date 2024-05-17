namespace TransactionalBox.Outbox
{
    public interface IEntityFrameworkTransaction
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}
