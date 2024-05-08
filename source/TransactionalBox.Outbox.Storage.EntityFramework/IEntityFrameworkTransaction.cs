namespace TransactionalBox.Outbox.Storage.EntityFramework
{
    public interface IEntityFrameworkTransaction
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}
