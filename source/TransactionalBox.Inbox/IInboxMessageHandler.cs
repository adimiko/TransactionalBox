using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox.Inbox
{
    public interface IInboxMessageHandler<TInboxMessage>
        where TInboxMessage : InboxMessage, new() 
    {
        Task Handle(TInboxMessage message, IExecutionContext executionContext);
    }
}
