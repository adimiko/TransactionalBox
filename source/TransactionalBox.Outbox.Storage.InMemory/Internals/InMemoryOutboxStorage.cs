using TransactionalBox.Outbox.Internals;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.KeyedInMemoryLock;
using TransactionalBox.Outbox.Internals.Contracts;
using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Storage.InMemory.Internals
{
    internal sealed class InMemoryOutboxStorage : IOutboxStorage, IOutboxWorkerStorage, IOutboxStorageReadOnly
    {
        private static readonly List<OutboxMessage> _outboxMessages = new List<OutboxMessage>();

        public IReadOnlyCollection<OutboxMessage> OutboxMessages => _outboxMessages.AsReadOnly();

        private readonly IKeyedInMemoryLock _keyedInMemoryLock;

        public InMemoryOutboxStorage(IKeyedInMemoryLock keyedInMemoryLock)
        {
            _keyedInMemoryLock = keyedInMemoryLock;
        }

        public async Task Add(OutboxMessage message)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                _outboxMessages.Add(message);
            }
        }

        public async Task AddRange(IEnumerable<OutboxMessage> messages)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                _outboxMessages.AddRange(messages);
            }
        }

        public async Task<IEnumerable<OutboxMessage>> GetMarkedMessages(JobId jobId)
        {
            IEnumerable<OutboxMessage> messages;

            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                messages = _outboxMessages.Where(x => !x.IsProcessed && x.JobId == jobId.ToString());
            }

            return messages;
        }

        public async Task MarkAsProcessed(JobId jobId, DateTime processedUtc)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                var messages = _outboxMessages.Where(x => !x.IsProcessed && x.JobId == jobId.ToString());

                foreach (var message in messages)
                {
                    message.IsProcessed = true;
                    message.JobId = null;
                    message.LockUtc = null;
                }
            }
        }

        public async Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            List<OutboxMessage> messages;

            using (await _keyedInMemoryLock.Acquire(nameof(InMemoryOutboxStorage)))
            {
                messages = _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => !x.IsProcessed && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ToList();

                foreach (var message in messages)
                {
                    message.JobId = jobId.ToString();
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
