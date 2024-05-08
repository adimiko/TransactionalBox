using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TransactionalBox.Outbox.Storage.EntityFramework.Internals
{
    internal sealed class EntityFrameworkTransaction : IEntityFrameworkTransaction
    {
        private readonly DbContext _dbContext;

        private readonly IDbContextTransaction _transaction;

        public EntityFrameworkTransaction(
            DbContext dbContext,
            IDbContextTransaction transaction) 
        {
            _dbContext = dbContext;
            _transaction = transaction;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            await _transaction.CommitAsync().ConfigureAwait(false);

            //TODO hook
            //TODO obserability
            // TODO użyć interfejsu z warstwy wyżej, która wykona ten kod :)
            // hook
            // observability
            //TransactionCommited service i tyle :P
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync().ConfigureAwait(false);
        }
    }
}
