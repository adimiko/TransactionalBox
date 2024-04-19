using Microsoft.EntityFrameworkCore;
using TransactionalBox.DistributedLock;
using TransactionalBox.Inbox.Internals.Contracts;
using TransactionalBox.Base.Inbox.StorageModel.Internals;
using System.Data;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;

namespace TransactionalBox.Inbox.Storage.EntityFramework.Internals
{
    internal sealed class EntityFrameworkInboxStorage : IInboxStorage
    {
        private readonly DbContext _dbContext;

        private readonly IDistributedLock<InboxDistributedLock> _distributedLock;

        public EntityFrameworkInboxStorage(
            DbContext dbContext,
            IDistributedLock<InboxDistributedLock> distributedLock) 
        {
            _dbContext = dbContext;
            _distributedLock = distributedLock;
        }

        public async Task<InboxMessage?> GetMessage(JobId jobId, JobName jobName, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            InboxMessage? message = null;

            int rowCount = 0;

            await using (await _distributedLock.Acquire(jobName.ToString(), timeProvider, lockTimeout, TimeSpan.FromMicroseconds(50)).ConfigureAwait(false))
            {
                var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

                using (var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted).ConfigureAwait(false))
                {
                    rowCount = await _dbContext.Set<InboxMessage>()
                    .OrderBy(x => x.OccurredUtc)
                    .Where(x => !x.IsProcessed && (x.LockUtc == null || x.LockUtc <= nowUtc))
                    .Take(1)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.LockUtc, nowUtc + lockTimeout)
                        .SetProperty(x => x.JobId, jobId.ToString()))
                    .ConfigureAwait(false);

                    if (rowCount > 0)
                    {
                        message = await _dbContext.Set<InboxMessage>()
                        .Where(x => !x.IsProcessed && x.JobId == jobId.ToString())
                        .OrderBy(x => x.OccurredUtc)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);
                    }

                    await transaction.CommitAsync().ConfigureAwait(false);
                }
            }

            if (message is not null) 
            {
                message.IsProcessed = true;
                message.LockUtc = null;
                message.JobId = null;
            }
            
            return message;
        }
    }
}
