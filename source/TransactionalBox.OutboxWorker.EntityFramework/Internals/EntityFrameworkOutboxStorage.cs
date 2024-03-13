﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<OutboxMessage>> GetMessages(int batchSize, DateTime nowUtc, DateTime lockUtc, string machineName)
        {
            //TODO (Check) Is update from select (without any hints) okay for a race condition ?
            await _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => x.ProcessedUtc == null && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(x => x.LockUtc, lockUtc)
                    .SetProperty(x => x.MachineName, machineName));

            var messages = await _outboxMessages
                .Where(x => x.ProcessedUtc == null && x.MachineName == machineName)
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
