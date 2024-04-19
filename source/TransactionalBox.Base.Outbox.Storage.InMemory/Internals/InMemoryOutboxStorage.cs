using TransactionalBox.Outbox.Internals;
using TransactionalBox.Base.Outbox.StorageModel.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.Base.BackgroundService.Internals.Contexts.JobExecution.ValueObjects;

namespace TransactionalBox.Base.Outbox.Storage.InMemory.Internals
{
    internal sealed class InMemoryOutboxStorage : IOutboxStorage, IOutboxWorkerStorage, IOutboxStorageReadOnly
    {
        //TODO semaphoreSlim
        //TODO DistributedLock
        private static readonly List<OutboxMessage> _outboxMessages = new List<OutboxMessage>();

        public IReadOnlyCollection<OutboxMessage> OutboxMessages => _outboxMessages.AsReadOnly();

        public Task Add(OutboxMessage message)
        {
            _outboxMessages.Add(message);

            return Task.CompletedTask;
        }

        public Task AddRange(IEnumerable<OutboxMessage> messages)
        {
            _outboxMessages.AddRange(messages);

            return Task.CompletedTask;
        }

        public Task<IEnumerable<OutboxMessage>> GetMarkedMessages(JobId jobId)
        {
            var messages = _outboxMessages.Where(x => !x.IsProcessed && x.JobId == jobId.ToString());

            return Task.FromResult(messages);
        }

        public Task MarkAsProcessed(JobId jobId, DateTime processedUtc)
        {
            var messages = _outboxMessages.Where(x => !x.IsProcessed && x.JobId == jobId.ToString());

            foreach (var message in messages) 
            {
                message.IsProcessed = true;
                message.JobId = null;
                message.LockUtc = null;
            }

            return Task.CompletedTask;
        }

        public Task<int> MarkMessages(JobId jobId, JobName jobName, int batchSize, TimeProvider timeProvider, TimeSpan lockTimeout)
        {
            var nowUtc = timeProvider.GetUtcNow().UtcDateTime;

            var messages = _outboxMessages
                .OrderBy(x => x.OccurredUtc)
                .Where(x => !x.IsProcessed && (x.LockUtc == null || x.LockUtc <= nowUtc))
                .Take(batchSize)
                .ToList();

            foreach(var message in messages) 
            {
                message.JobId = jobId.ToString();
                message.LockUtc = nowUtc + lockTimeout;
            }

            return Task.FromResult(messages.Count);
        }

        public Task<int> RemoveProcessedMessages(int batchSize)
        {
            var messagesToRemove = _outboxMessages
                .Where(x => x.IsProcessed)
                .Take(batchSize);

            foreach (var message in messagesToRemove) 
            {
                _outboxMessages.Remove(message);
            }
            
            return Task.FromResult(messagesToRemove.Count());
        }
    }
}
