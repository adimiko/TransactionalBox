using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.Inbox.Internals.Contracts;
using TransactionalBox.Base.Inbox.StorageModel.Internals;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;
using TransactionalBox.KeyedInMemoryLock;

namespace TransactionalBox.Base.Inbox.Storage.InMemory.Internals
{
    internal sealed class InMemoryInboxStorage : IInboxStorage, IInboxWorkerStorage, IInboxStorageReadOnly
    {
        private static readonly List<InboxMessage> _inboxMessages = new List<InboxMessage>();

        private static readonly List<IdempotentInboxKey> _idempotentInboxKeys = new List<IdempotentInboxKey>();

        public IReadOnlyCollection<InboxMessage> InboxMessages => _inboxMessages;

        public IReadOnlyCollection<IdempotentInboxKey> IdempotentInboxKeys => _idempotentInboxKeys;

        private readonly IKeyedInMemoryLock _keyedInMemoryLock;

        public InMemoryInboxStorage(IKeyedInMemoryLock keyedInMemoryLock)
        {
            _keyedInMemoryLock = keyedInMemoryLock;
        }

        public async Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessage> messages, IEnumerable<IdempotentInboxKey> idempotentInboxKeys)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(_inboxMessages)))
            using (await _keyedInMemoryLock.Acquire(nameof(_idempotentInboxKeys)))
            {
                _inboxMessages.AddRange(messages);

                _idempotentInboxKeys.AddRange(idempotentInboxKeys);
            }

            return AddRangeToInboxStorageResult.Success;
        }

        public async Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessage> messages)
        {
            IEnumerable<IdempotentInboxKey> idempotentInboxKeys;

            using (await _keyedInMemoryLock.Acquire(nameof(_idempotentInboxKeys)))
            {
                var ids = messages.Select(x => x.Id);

                idempotentInboxKeys = _idempotentInboxKeys.Where(x => ids.Contains(x.Id));
            }

            return idempotentInboxKeys;
        }

        public async Task<InboxMessage?> GetMessage(JobId jobId, JobName jobName, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            InboxMessage? message;

            using (await _keyedInMemoryLock.Acquire(nameof(_inboxMessages)))
            {
                message = _inboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => !x.IsProcessed && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .FirstOrDefault();

                if (message is not null)
                {
                    message.IsProcessed = true;
                    message.LockUtc = null;
                    message.JobId = null;
                }
            }

            return message;
        }

        public async Task<int> RemoveExpiredIdempotencyKeys(int batchSize, DateTime nowUtc)
        {
            List<IdempotentInboxKey> expiredIdempotentKeys;

            using (await _keyedInMemoryLock.Acquire(nameof(_idempotentInboxKeys)))
            {
                expiredIdempotentKeys = _idempotentInboxKeys.Where(x => x.ExpirationUtc <= nowUtc).Take(batchSize).ToList();

                foreach (var key in expiredIdempotentKeys)
                {
                    _idempotentInboxKeys.Remove(key);
                }
            }

            return expiredIdempotentKeys.Count;
        }

        public async Task<int> RemoveProcessedMessages(int batchSize)
        {
            List<InboxMessage> messages;

            using (await _keyedInMemoryLock.Acquire(nameof(_inboxMessages)))
            {
                messages = _inboxMessages.Where(x => x.IsProcessed).Take(batchSize).ToList();

                foreach (var message in messages)
                {
                    _inboxMessages.Remove(message);
                }
            }

            return messages.Count;
        }
    }
}
