namespace TransactionalBox.Internals.Outbox.OutboxMessageDefinitions
{
    internal interface IOutboxMessageDefinition
    {
        string? Receiver { get; }
    }
}
