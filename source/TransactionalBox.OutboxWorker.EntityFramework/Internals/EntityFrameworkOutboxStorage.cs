using Microsoft.EntityFrameworkCore;
using System.Data;
using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxStorage : IOutboxStorage
    {
        private const IsolationLevel _isolationLevel = IsolationLevel.ReadCommitted;

        private readonly DbContext _dbContext;

        private readonly DbSet<OutboxMessage> _outboxMessages;

        private readonly EntityFrameworkOutboxLockStorage _distributedLock;

        public EntityFrameworkOutboxStorage(
            DbContext dbContext,
            EntityFrameworkOutboxLockStorage frameworkOutboxLockStorage) 
        {
            _dbContext = dbContext;
            _outboxMessages = dbContext.Set<OutboxMessage>();
            _distributedLock = frameworkOutboxLockStorage;
        }
        public async Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, DateTime nowUtc, DateTime lockUtc)
        {
            int rowCount = 0;

            await _distributedLock.Acquire(jobName.ToString());

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                rowCount = await _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => x.ProcessedUtc == null && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.LockUtc, lockUtc)
                    .SetProperty(x => x.JobId, jobId.ToString()));

                await transaction.CommitAsync();
            }

            await _distributedLock.Release();

            return rowCount;
        }

        public async Task<IEnumerable<OutboxMessage>> GetMessages(JobId jobId)
        {
            IEnumerable<OutboxMessage> messages;

            using (var transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel))
            {
                messages = await _outboxMessages
                    .AsNoTracking()
                    .Where(x => x.ProcessedUtc == null && x.JobId == jobId.ToString())
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
                    .Where(x => x.ProcessedUtc == null && x.JobId == jobId.ToString())
                    .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ProcessedUtc, processedUtc));

                await transaction.CommitAsync();
            }
        }
    }
}
