namespace TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob.TransportMessageFactories
{
    internal sealed class TransportMessage
    {
        internal required string Topic { get; init; }

        internal required byte[] Payload { get; init; }
    }
}
