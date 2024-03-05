namespace TransactionalBox.Inbox
{
    public interface IInbox<T> where T : InboxMessage, new()
    {
        Task Handle(T message, CancellationToken cancellationToken);
    }
}
