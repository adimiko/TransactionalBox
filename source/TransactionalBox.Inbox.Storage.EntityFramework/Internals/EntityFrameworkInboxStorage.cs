using Microsoft.EntityFrameworkCore;
using TransactionalBox.BackgroundServiceBase.Internals.ValueObjects;
using TransactionalBox.DistributedLock;
using TransactionalBox.Inbox.Internals.Contracts;
using TransactionalBox.InboxBase.StorageModel.Internals;

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

        public async Task<InboxMessage?> GetMessage(JobId jobId, JobName jobName, DateTime nowUtc, TimeSpan lockTimeout)
        {
            await _distributedLock.Acquire(jobName.ToString(), nowUtc, lockTimeout, TimeSpan.FromMicroseconds(50));

            var message = await _dbContext.Set<InboxMessage>()
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.OccurredUtc)
            .FirstOrDefaultAsync();

            await _distributedLock.Release();

            if (message is not null) 
            {
                message.IsProcessed = true;
            }
            
            return message;
        }
    }
}
