namespace TransactionalBox.Inbox
{
    public interface IInboxMessageHandler<TInboxMessage> 
        where TInboxMessage : InboxMessageBase, new()
    {
        Task Handle(TInboxMessage message, CancellationToken cancellationToken);
    }
}
