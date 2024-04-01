namespace TransactionalBox.Outbox.Serialization
{
    public interface IOutboxSerializer
    {
        string Serialize<T>(T outboxMessagePayload) where T : class, IOutboxMessagePayload;
    }
}
