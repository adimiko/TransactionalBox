using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.EntityFrameworkCore.Internals
{
    internal sealed class EntityFrameworkTransaction : IEntityFrameworkTransaction
    {
        private readonly DbContext _dbContext;

        private readonly IDbContextTransaction _transaction;

        private readonly ITranactionCommited _tranactionCommited;

        public EntityFrameworkTransaction(
            DbContext dbContext,
            IDbContextTransaction transaction,
            ITranactionCommited tranactionCommited) 
        {
            _dbContext = dbContext;
            _transaction = transaction;
            _tranactionCommited = tranactionCommited;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);

            await _transaction.CommitAsync().ConfigureAwait(false);

            await _tranactionCommited.Commited().ConfigureAwait(false);
            //TODO notify when transaction is commited (hook & obserability)
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync().ConfigureAwait(false);
        }
    }
}
