﻿using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Storage.EntityFramework.Internals
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

        public Task AddRange(IEnumerable<OutboxMessage> messages)
        {
            return _outbox.AddRangeAsync(messages);
        }
    }
}
