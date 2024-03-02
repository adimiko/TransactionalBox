namespace TransactionalBox.Outbox
{
    public interface IOutboxSender
    {
        Task Send<TMessage>(TMessage message, string receiver, DateTime occurredUtc) where TMessage : MessageBase;
    }
}
