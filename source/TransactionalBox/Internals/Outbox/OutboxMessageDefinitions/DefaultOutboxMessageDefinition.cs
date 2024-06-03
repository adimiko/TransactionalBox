namespace TransactionalBox.Internals.Outbox.OutboxMessageDefinitions
{
    internal sealed class DefaultOutboxMessageDefinition : IOutboxMessageDefinition
    {
        public string? Receiver { get; private set; } = null;
    }
}
