namespace TransactionalBox.End2EndTests.SeedWork.Outbox
{
    internal sealed class SendableMessage : OutboxMessage
    {
        public required string Message { get; init; }
    }
}
