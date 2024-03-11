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

        public async Task<IEnumerable<OutboxMessage>> GetMessages()
        {
            var messages = await _outboxMessages.Where(x => x.ProcessedUtc == null).ToListAsync();

            return messages;
        }

        public Task MarkAsProcessed(IEnumerable<OutboxMessage> messages, DateTime processedUtc)
        {
            foreach (var message in messages) 
            {
                message.ProcessedUtc = processedUtc;
            }

            return _dbContext.SaveChangesAsync();
        }
    }
}
