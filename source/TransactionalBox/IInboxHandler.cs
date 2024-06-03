namespace TransactionalBox
{
    public interface IInboxHandler<TInboxMessage>
        where TInboxMessage : InboxMessage, new() 
    {
        /// <summary>
        /// Handle a message from inbox.
        /// </summary>
        Task Handle(TInboxMessage message, IExecutionContext executionContext);
    }
}
