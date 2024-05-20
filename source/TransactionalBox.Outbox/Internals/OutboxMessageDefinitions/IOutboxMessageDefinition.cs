namespace TransactionalBox.Outbox.Internals.OutboxMessageDefinitions
{
    internal interface IOutboxMessageDefinition
    {
        string? Receiver { get; }
    }
}
