using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Storage.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxStorage : IOutboxStorage
    {
        private readonly DbSet<OutboxMessageStorage> _outbox;

        public EntityFrameworkOutboxStorage(DbContext dbContext)
        {
            _outbox = dbContext.Set<OutboxMessageStorage>();
        }

        public Task Add(OutboxMessageStorage message)
        {
            return _outbox.AddAsync(message).AsTask();
        }

        public Task AddRange(IEnumerable<OutboxMessageStorage> messages)
        {
            return _outbox.AddRangeAsync(messages);
        }
    }
}
