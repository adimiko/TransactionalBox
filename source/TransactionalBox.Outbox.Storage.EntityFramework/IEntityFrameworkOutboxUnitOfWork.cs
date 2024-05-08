namespace TransactionalBox.Outbox.Storage.EntityFramework
{
    public interface IEntityFrameworkOutboxUnitOfWork
    {
        Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
