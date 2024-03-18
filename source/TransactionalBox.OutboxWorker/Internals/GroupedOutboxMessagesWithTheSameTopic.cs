using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    public sealed class GroupedOutboxMessagesWithTheSameTopic
    {
        public required string Topic { get; init; }

        public required IEnumerable<OutboxMessage> Messages { get; init; }
    }
}
