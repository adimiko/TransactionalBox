namespace TransactionalBox.OutboxWorker.Internals
{
    public sealed class TransportMessage
    {
        public required string Topic { get; init; }

        public required string Payload { get; init; }
    }
}
