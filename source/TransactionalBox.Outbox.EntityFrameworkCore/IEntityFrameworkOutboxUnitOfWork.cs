namespace TransactionalBox
{
    public interface IEntityFrameworkOutboxUnitOfWork
    {
        Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
