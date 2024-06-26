﻿using TransactionalBox.Internals.Inbox.Storage.ContractsToImplement;
using TransactionalBox.Internals.InternalPackages.KeyedInMemoryLock;

namespace TransactionalBox.Internals.Inbox.Storage.InMemory
{
    internal sealed class InMemoryInboxStorage : IAddMessagesToInboxRepository, ICleanUpIdempotencyKeysRepository, ICleanUpInboxRepository, IProcessMessageRepository, IInboxStorageReadOnly
    {
        private static readonly List<InboxMessageStorage> _inboxMessages = new List<InboxMessageStorage>();

        private static readonly List<IdempotentInboxKey> _idempotentInboxKeys = new List<IdempotentInboxKey>();

        public IReadOnlyCollection<InboxMessageStorage> InboxMessages => _inboxMessages;

        public IReadOnlyCollection<IdempotentInboxKey> IdempotentInboxKeys => _idempotentInboxKeys;

        private readonly IKeyedInMemoryLock _keyedInMemoryLock;

        public InMemoryInboxStorage(IKeyedInMemoryLock keyedInMemoryLock)
        {
            _keyedInMemoryLock = keyedInMemoryLock;
        }

        public async Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessageStorage> messages, IEnumerable<IdempotentInboxKey> idempotentInboxKeys)
        {
            using (await _keyedInMemoryLock.Acquire(nameof(_inboxMessages)))
            using (await _keyedInMemoryLock.Acquire(nameof(_idempotentInboxKeys)))
            {
                _inboxMessages.AddRange(messages);

                _idempotentInboxKeys.AddRange(idempotentInboxKeys);
            }

            return AddRangeToInboxStorageResult.Success;
        }

        public async Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessageStorage> messages)
        {
            IEnumerable<IdempotentInboxKey> idempotentInboxKeys;

            using (await _keyedInMemoryLock.Acquire(nameof(_idempotentInboxKeys)))
            {
                var ids = messages.Select(x => x.Id);

                idempotentInboxKeys = _idempotentInboxKeys.Where(x => ids.Contains(x.Id));
            }

            return idempotentInboxKeys;
        }

        public async Task<InboxMessageStorage?> GetMessage(Guid hookId, string hookName, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            InboxMessageStorage? message;

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
            List<InboxMessageStorage> messages;

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
