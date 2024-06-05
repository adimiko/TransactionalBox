namespace TransactionalBox.Internals.Outbox.OutboxDefinitions
{
    internal sealed class DefaultOutboxMessageDefinition : IOutboxDefinition
    {
        public string? Receiver { get; private set; } = null;
    }
}
