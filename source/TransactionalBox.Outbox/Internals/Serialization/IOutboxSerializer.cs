namespace TransactionalBox.Outbox.Internals.Serialization
{
    internal interface IOutboxSerializer
    {
        string Serialize<T>(T outboxMessagePayload) where T : class, IOutboxMessagePayload;
    }
}
