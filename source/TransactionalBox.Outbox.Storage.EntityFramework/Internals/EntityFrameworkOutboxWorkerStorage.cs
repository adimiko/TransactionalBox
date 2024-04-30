using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.DistributedLock;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.Outbox.Internals.Contracts;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Storage.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxWorkerStorage : IOutboxWorkerStorage
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<OutboxMessage> _outboxMessages;

        private readonly IDistributedLock<OutboxDistributedLock> _distributedLock;

        public EntityFrameworkOutboxWorkerStorage(
            DbContext dbContext,
            IDistributedLock<OutboxDistributedLock> distributedLock)
        {
            _dbContext = dbContext;
            _outboxMessages = dbContext.Set<OutboxMessage>();
            _distributedLock = distributedLock;
        }
        public async Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            int rowCount = 0;

            await using (await _distributedLock.Acquire(jobName.ToString(), timeProvider, lockTimeout, TimeSpan.FromMicroseconds(50)).ConfigureAwait(false))
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
                        .SetProperty(x => x.JobId, jobId.ToString()));

                    await transaction.CommitAsync();
                }
            }

            return rowCount;
        }

        public async Task<IEnumerable<OutboxMessage>> GetMarkedMessages(JobId jobId)
        {
            IEnumerable<OutboxMessage> messages;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                messages = await _outboxMessages
                    .AsNoTracking()
                    .Where(x => !x.IsProcessed && x.JobId == jobId.ToString())
                    .ToListAsync();

                await transaction.CommitAsync();
            }

            return messages;
        }

        public async Task MarkAsProcessed(JobId jobId, DateTime processedUtc)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                await _outboxMessages
                    .Where(x => !x.IsProcessed && x.JobId == jobId.ToString())
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.IsProcessed, true)
                        .SetProperty(x => x.JobId, (string?)null)
                        .SetProperty(x => x.LockUtc, (DateTime?)null));

                await transaction.CommitAsync();
            }
        }

        public async Task<int> RemoveProcessedMessages(int batchSize)
        {
            int rowCount = 0;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                rowCount = await _outboxMessages
                    .Where(x => x.IsProcessed)
                    .Take(batchSize)
                    .ExecuteDeleteAsync();

                await transaction.CommitAsync();
            }

            return rowCount;
        }
    }
}
