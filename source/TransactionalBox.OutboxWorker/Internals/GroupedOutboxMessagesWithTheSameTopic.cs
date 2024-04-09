using TransactionalBox.Base.Outbox.StorageModel.Internals;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class GroupedOutboxMessagesWithTheSameTopic
    {
        internal required string Topic { get; init; }

        internal required IEnumerable<OutboxMessage> Messages { get; init; }
    }
}
