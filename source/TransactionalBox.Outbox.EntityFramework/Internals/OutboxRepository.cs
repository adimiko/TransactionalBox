using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals;

namespace TransactionalBox.Outbox.EntityFramework.Internals
{
    internal sealed class OutboxRepository : IOutboxRepository
    {
        private readonly DbSet<OutboxMessage> _outbox;

        public OutboxRepository(DbContext dbContext)
        {
            _outbox = dbContext.Set<OutboxMessage>();
        }

        public Task Add(OutboxMessage message)
        {
            return _outbox.AddAsync(message).AsTask();
        }
    }
}
