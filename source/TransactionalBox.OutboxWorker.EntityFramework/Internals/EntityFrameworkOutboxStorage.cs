using Microsoft.EntityFrameworkCore;
using TransactionalBox.OutboxBase.StorageModel;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxStorage : IOutboxStorage
    {
        private static Mutex _mutex = new Mutex();

        private readonly DbContext _dbContext;

        private readonly DbSet<OutboxMessage> _outboxMessages;

        public EntityFrameworkOutboxStorage(DbContext dbContext) 
        {
            _dbContext = dbContext;
            _outboxMessages = dbContext.Set<OutboxMessage>();
        }

        public async Task<IEnumerable<OutboxMessage>> GetMessages(string jobExecutionId, int batchSize, DateTime nowUtc, DateTime lockUtc)
        {
            // (Database performance) Added mutex because Entity Framework does not support skipping locked rows

            _mutex.WaitOne();

            var rowCount = await _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => x.ProcessedUtc == null && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.LockUtc, lockUtc)
                    .SetProperty(x => x.JobExecutionId, jobExecutionId));

            _mutex.ReleaseMutex();

            if (rowCount < 1)
            {
                return Enumerable.Empty<OutboxMessage>();
            }

            var messages = await _outboxMessages
                .AsNoTracking()
                .Where(x => x.ProcessedUtc == null && x.JobExecutionId == jobExecutionId)
                .ToListAsync();

            return messages;
        }

        public Task MarkAsProcessed(IEnumerable<OutboxMessage> messages, DateTime processedUtc)
        {
            var ids = messages.Select(x => x.Id);
            
            return _outboxMessages
                    .Where(x => x.ProcessedUtc == null && ids.Contains(x.Id))
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(x => x.ProcessedUtc, processedUtc));
        }
    }
}
