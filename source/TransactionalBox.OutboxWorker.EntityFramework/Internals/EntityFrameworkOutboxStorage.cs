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

        public Task MarkAsProcessed(IEnumerable<OutboxMessage> messages)
        {
            //TODO TimeProvider
            var dateTime = DateTime.UtcNow;

            foreach (var message in messages) 
            {
                message.ProcessedUtc = dateTime;
            }

            return _dbContext.SaveChangesAsync();
        }
    }
}
