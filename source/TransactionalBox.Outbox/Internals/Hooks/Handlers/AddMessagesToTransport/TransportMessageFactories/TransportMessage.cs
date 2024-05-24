namespace TransactionalBox.Outbox.Internals.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories
{
    internal sealed class TransportMessage
    {
        internal required string Topic { get; init; }

        internal required byte[] Payload { get; init; }

        internal required string ContentType { get; init; }
    }
}
