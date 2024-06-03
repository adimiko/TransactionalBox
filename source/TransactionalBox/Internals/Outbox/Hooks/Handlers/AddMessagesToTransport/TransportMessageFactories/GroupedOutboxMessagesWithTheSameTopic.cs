using TransactionalBox.Internals.Outbox.Storage;

namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories
{
    internal sealed class GroupedOutboxMessagesWithTheSameTopic
    {
        internal required string Topic { get; init; }

        internal required IEnumerable<OutboxMessageStorage> Messages { get; init; }
    }
}
