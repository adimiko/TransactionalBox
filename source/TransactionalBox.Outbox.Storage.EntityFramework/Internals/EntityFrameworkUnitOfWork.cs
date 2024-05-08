using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TransactionalBox.Outbox.Storage.EntityFramework.Internals
{
    internal sealed class EntityFrameworkUnitOfWork : IEntityFrameworkOutboxUnitOfWork
    {
        private readonly DbContext _dbContext;

        public EntityFrameworkUnitOfWork(DbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<IEntityFrameworkTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

            return new EntityFrameworkTransaction(_dbContext, transaction);
        }
    }
}
