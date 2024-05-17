namespace TransactionalBox.Outbox
{
    public interface IEntityFrameworkOutboxUnitOfWork
    {
        Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
