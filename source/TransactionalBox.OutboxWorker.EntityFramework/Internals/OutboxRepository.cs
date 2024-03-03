using Microsoft.EntityFrameworkCore;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.EntityFramework.Internals
{
    internal sealed class OutboxRepository : IOutboxRepository
    {
        private readonly DbSet<OutboxMessage> _outboxMessages;

        public OutboxRepository(DbContext dbContext) 
        {
            _outboxMessages = dbContext.Set<OutboxMessage>();
        }

        public async Task<IEnumerable<OutboxMessage>> GetMessages()
        {
            var messages = await _outboxMessages.ToListAsync();

            return messages;
        }
    }
}
