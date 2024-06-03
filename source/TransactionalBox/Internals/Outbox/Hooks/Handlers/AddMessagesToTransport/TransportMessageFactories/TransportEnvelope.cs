namespace TransactionalBox.Internals.Outbox.Hooks.Handlers.AddMessagesToTransport.TransportMessageFactories
{
    internal sealed class TransportEnvelope
    {
        internal required string Topic { get; init; }

        internal required byte[] Payload { get; init; }

        internal required string Compression { get; init; }
    }
}
