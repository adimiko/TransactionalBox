using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.Outbox.EntityFramework.Internals
{
    internal sealed class EntityFrameworkOutboxStorage : IOutboxStorage
    {
        private readonly DbSet<OutboxMessage> _outbox;

        public EntityFrameworkOutboxStorage(DbContext dbContext)
        {
            _outbox = dbContext.Set<OutboxMessage>();
        }

        public Task Add(OutboxMessage message)
        {
            return _outbox.AddAsync(message).AsTask();
        }
    }
}
