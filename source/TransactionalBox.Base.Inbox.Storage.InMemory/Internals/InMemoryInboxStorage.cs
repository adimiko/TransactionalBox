﻿using TransactionalBox.InboxWorker.Internals.Contracts;
using TransactionalBox.Inbox.Internals.Contracts;
using TransactionalBox.Base.Inbox.StorageModel.Internals;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;

namespace TransactionalBox.Base.Inbox.Storage.InMemory.Internals
{
    internal sealed class InMemoryInboxStorage : IInboxStorage, IInboxWorkerStorage, IInboxStorageReadOnly
    {
        private static readonly List<InboxMessage> _inboxMessages = new List<InboxMessage>();

        private static readonly List<IdempotentInboxKey> _idempotentInboxKeys = new List<IdempotentInboxKey>();

        public IReadOnlyCollection<InboxMessage> InboxMessages => _inboxMessages;

        public IReadOnlyCollection<IdempotentInboxKey> IdempotentInboxKeys => _idempotentInboxKeys;

        public Task<AddRangeToInboxStorageResult> AddRange(IEnumerable<InboxMessage> messages, IEnumerable<IdempotentInboxKey> idempotentInboxKeys)
        {
            _inboxMessages.AddRange(messages);
            
            _idempotentInboxKeys.AddRange(idempotentInboxKeys);

            return Task.FromResult(AddRangeToInboxStorageResult.Success);
        }

        public Task<IEnumerable<IdempotentInboxKey>> GetExistIdempotentInboxKeysBasedOn(IEnumerable<InboxMessage> messages)
        {
            var ids = messages.Select(x => x.Id);

            var idempotentInboxKeys = _idempotentInboxKeys.Where(x => ids.Contains(x.Id));

            return Task.FromResult(idempotentInboxKeys);
        }

        public Task<InboxMessage?> GetMessage(JobId jobId, JobName jobName, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            //TODO mark message and process
            var message = _inboxMessages
            .Where(x => !x.IsProcessed)
            .OrderBy(x => x.OccurredUtc)
            .FirstOrDefault();

            if (message is not null)
            {
                message.IsProcessed = true;
                message.LockUtc = null;
                message.JobId = null;
            }

            return Task.FromResult(message);
        }

        public Task<int> RemoveExpiredIdempotencyKeys(int batchSize, DateTime nowUtc)
        {
            var expiredIdempotentKeys = _idempotentInboxKeys.Where(x => x.ExpirationUtc <= nowUtc).Take(batchSize).ToList();

            foreach (var key in expiredIdempotentKeys)
            {
                _idempotentInboxKeys.Remove(key);
            }

            return Task.FromResult(expiredIdempotentKeys.Count);
        }

        public Task<int> RemoveProcessedMessages(int batchSize)
        {
            var messages = _inboxMessages.Where(x => x.IsProcessed).Take(batchSize).ToList();

            foreach (var message in messages)
            {
                _inboxMessages.Remove(message);
            }

            return Task.FromResult(messages.Count);
        }
    }
}
