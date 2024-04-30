using TransactionalBox.Outbox.Internals.Storage;

namespace TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories
{
    internal sealed class GroupedOutboxMessagesWithTheSameTopic
    {
        internal required string Topic { get; init; }

        internal required IEnumerable<OutboxMessage> Messages { get; init; }
    }
}
