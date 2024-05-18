using TransactionalBox.Internals.KeyedInMemoryLock;

namespace TransactionalBox.Outbox.Internals.Storage.InMemory
{
    internal sealed class InMemoryOutboxStorage : IOutboxStorage, IAddMessagesToTransportRepository, ICleanUpOutboxRepository, IOutboxStorageReadOnly
    {
        private static readonly List<OutboxMessageStorage> _outboxMessages = new List<OutboxMessageStorage>();

        public IReadOnlyCollection<OutboxMessageStorage> OutboxMessages => _outboxMessages.AsReadOnly();

        private readonly IKeyedInMemoryLock _keyedInMemoryLock;

        private readonly ITranactionCommited _tranactionCommited;

        public InMemoryOutboxStorage(
            IKeyedInMemoryLock keyedInMemoryLock,
            ITranactionCommited tranactionCommited)
        {
            _keyedInMemoryLock = keyedInMemoryLock;
            _tranactionCommited = tranactionCommited;
        }

        public async Task Add(OutboxMessageStorage message)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                _outboxMessages.Add(message);
            }

            await _tranactionCommited.Commited().ConfigureAwait(false);
        }

        public async Task AddRange(IEnumerable<OutboxMessageStorage> messages)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                _outboxMessages.AddRange(messages);
            }

            await _tranactionCommited.Commited().ConfigureAwait(false);
        }

        public async Task<IEnumerable<OutboxMessageStorage>> GetMarkedMessages(Guid hookId)
        {
            IEnumerable<OutboxMessageStorage> messages;

            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                messages = _outboxMessages.Where(x => !x.IsProcessed && x.JobId == hookId.ToString());
            }

            return messages;
        }

        public async Task MarkAsProcessed(Guid hookId, DateTime processedUtc)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                var messages = _outboxMessages.Where(x => !x.IsProcessed && x.JobId == hookId.ToString());

                foreach (var message in messages)
                {
                    message.IsProcessed = true;
                    message.JobId = null;
                    message.LockUtc = null;
                }
            }
        }

        public async Task<int> MarkMessages(Guid hookId, string hookName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            List<OutboxMessageStorage> messages;

            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                messages = _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => !x.IsProcessed && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ToList();

                foreach (var message in messages)
                {
                    message.JobId = hookId.ToString();
                    message.LockUtc = nowUtc + lockTimeout;
                }
            }

            return messages.Count;
        }

        public async Task<int> RemoveProcessedMessages(int batchSize)
        {
            int count;

            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                var messagesToRemove = _outboxMessages
                .Where(x => x.IsProcessed)
                .Take(batchSize)
                .ToList();

                count = messagesToRemove.Count();

                foreach (var message in messagesToRemove)
                {
                    _outboxMessages.Remove(message);
                }
            }

            return count;
        }
    }
}
