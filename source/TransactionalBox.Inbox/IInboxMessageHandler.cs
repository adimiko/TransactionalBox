namespace TransactionalBox.Inbox
{
    public interface IInboxMessageHandler<TInboxMessage> 
        where TInboxMessage : IInboxMessage, new()
    {
        Task Handle(TInboxMessage message, CancellationToken cancellationToken);
    }
}
