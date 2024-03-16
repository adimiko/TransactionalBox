using Microsoft.EntityFrameworkCore;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxStorage : IOutboxStorage
    {
        private readonly DbContext _dbContext;

        private readonly DbSet<OutboxMessage> _outboxMessages;

        public EntityFrameworkOutboxStorage(DbContext dbContext) 
        {
            _dbContext = dbContext;
            _outboxMessages = dbContext.Set<OutboxMessage>();
        }

        public async Task<IEnumerable<OutboxMessage>> GetMessages(string jobId, int batchSize, DateTime nowUtc, DateTime lockUtc)
        {
            var rowCount = await _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => x.ProcessedUtc == null && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.LockUtc, lockUtc)
                    .SetProperty(x => x.JobId, jobId));

            if (rowCount < 1)
            {
                return Enumerable.Empty<OutboxMessage>();
            }

            var messages = await _outboxMessages
                .AsNoTracking()
                .Where(x => x.ProcessedUtc == null && x.JobId == jobId)
                .ToListAsync();

            return messages;
        }

        public Task MarkAsProcessed(string jobId, DateTime processedUtc)
        {            
            return _outboxMessages
                    .Where(x => x.ProcessedUtc == null && x.JobId == jobId)
                    .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.ProcessedUtc, processedUtc));
        }
    }
}
