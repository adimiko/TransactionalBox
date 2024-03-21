using TransactionalBox.OutboxBase.StorageModel;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class GroupedOutboxMessagesWithTheSameTopic
    {
        internal required string Topic { get; init; }

        internal required IEnumerable<OutboxMessage> Messages { get; init; }
    }
}
