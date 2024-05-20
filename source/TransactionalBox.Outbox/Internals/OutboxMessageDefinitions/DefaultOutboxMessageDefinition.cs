namespace TransactionalBox.Outbox.Internals.OutboxMessageDefinitions
{
    internal sealed class DefaultOutboxMessageDefinition : IOutboxMessageDefinition
    {
        public string? Receiver { get; private set; } = null;
    }
}
