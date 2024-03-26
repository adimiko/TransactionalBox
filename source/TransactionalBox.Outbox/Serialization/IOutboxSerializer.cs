namespace TransactionalBox.Outbox.Serialization
{
    public interface IOutboxSerializer
    {
        string Serialize<T>(T outboxMessage) where T : class, IOutboxMessage;
    }
}
