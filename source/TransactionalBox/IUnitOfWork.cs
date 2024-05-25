namespace TransactionalBox
{
    public interface IUnitOfWork
    {
        Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
