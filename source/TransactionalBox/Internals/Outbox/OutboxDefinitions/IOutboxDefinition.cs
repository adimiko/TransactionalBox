namespace TransactionalBox.Internals.Outbox.OutboxDefinitions
{
    internal interface IOutboxDefinition
    {
        string? Receiver { get; }
    }
}
