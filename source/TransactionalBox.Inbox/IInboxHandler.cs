using TransactionalBox.Inbox.Contexts;

namespace TransactionalBox
{
    public interface IInboxHandler<TInboxMessage>
        where TInboxMessage : InboxMessage, new() 
    {
        Task Handle(TInboxMessage message, IExecutionContext executionContext);
    }
}
