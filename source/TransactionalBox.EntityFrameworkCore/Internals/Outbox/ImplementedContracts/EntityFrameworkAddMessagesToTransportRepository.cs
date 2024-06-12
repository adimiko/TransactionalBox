using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.Internals.InternalPackages.DistributedLock;
using TransactionalBox.Internals.Outbox.Storage;
using TransactionalBox.Internals.Outbox.Storage.ContractsToImplement;

namespace TransactionalBox.EntityFrameworkCore.Internals.Outbox.ImplementedContracts
{
    internal sealed class EntityFrameworkAddMessagesToTransportRepository : IAddMessagesToTransportRepository
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<OutboxMessageStorage> _outboxMessages;

        private readonly IDistributedLock<OutboxDistributedLock> _distributedLock;

        public EntityFrameworkAddMessagesToTransportRepository(
            DbContext dbContext,
            IDistributedLock<OutboxDistributedLock> distributedLock)
        {
            _dbContext = dbContext;
            _outboxMessages = dbContext.Set<OutboxMessageStorage>();
            _distributedLock = distributedLock;
        }
        public async Task<int> MarkMessages(Guid hookId, string hookName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            int rowCount = 0;

            await using (await _distributedLock.Acquire(hookName, timeProvider, lockTimeout, TimeSpan.FromMicroseconds(50)).ConfigureAwait(false))
            {
                var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

                using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
                {
                    rowCount = await _outboxMessages
                    .OrderBy(x => x.OccurredUtc)
                    .Where(x => !x.IsProcessed && (x.LockUtc == null || x.LockUtc <= nowUtc))
                    .Take(batchSize)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.LockUtc, nowUtc + lockTimeout)
                        .SetProperty(x => x.JobId, hookId.ToString()));

                    await transaction.CommitAsync();
                }
            }

            return rowCount;
        }

        public async Task<IEnumerable<OutboxMessageStorage>> GetMarkedMessages(Guid hookId)
        {
            IEnumerable<OutboxMessageStorage> messages;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                messages = await _outboxMessages
                    .AsNoTracking()
                    .Where(x => !x.IsProcessed && x.JobId == hookId.ToString())
                    .ToListAsync();

                await transaction.CommitAsync();
            }

            return messages;
        }

        public async Task MarkAsProcessed(Guid hookId, DateTime processedUtc)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                await _outboxMessages
                    .Where(x => !x.IsProcessed && x.JobId == hookId.ToString())
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.IsProcessed, true)
                        .SetProperty(x => x.JobId, (string?)null)
                        .SetProperty(x => x.LockUtc, (DateTime?)null));

                await transaction.CommitAsync();
            }
        }
    }
}
