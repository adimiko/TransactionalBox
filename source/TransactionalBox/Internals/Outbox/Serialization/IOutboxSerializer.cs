namespace TransactionalBox.Internals.Outbox.Serialization
{
    internal interface IOutboxSerializer
    {
        string Serialize<T>(T outboxMessagePayload) where T : class, IOutboxMessagePayload;
    }
}
