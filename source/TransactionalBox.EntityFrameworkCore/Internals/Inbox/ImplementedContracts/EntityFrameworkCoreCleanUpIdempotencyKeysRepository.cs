using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.Internals.Inbox.Storage;
using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;

namespace TransactionalBox.EntityFrameworkCore.Internals.Inbox.ImplementedContracts
{
    internal sealed class EntityFrameworkCoreCleanUpIdempotencyKeysRepository : ICleanUpIdempotencyKeysRepository
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<IdempotentInboxKey> _idempotentInboxKeys;

        public EntityFrameworkCoreCleanUpIdempotencyKeysRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _idempotentInboxKeys = _dbContext.Set<IdempotentInboxKey>();
        }

        public async Task<int> RemoveExpiredIdempotencyKeys(int batchSize, DateTime nowUtc)
        {
            int numberOfDeletedRows;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel).ConfigureAwait(false))
            {
                numberOfDeletedRows = await _idempotentInboxKeys
                    .Where(x => x.ExpirationUtc <= nowUtc)
                    .Take(batchSize)
                    .ExecuteDeleteAsync()
                    .ConfigureAwait(false);

                await transaction.CommitAsync().ConfigureAwait(false);
            }

            return numberOfDeletedRows;
        }
    }
}
