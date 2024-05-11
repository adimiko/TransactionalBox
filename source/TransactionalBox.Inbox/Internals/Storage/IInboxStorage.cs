namespace TransactionalBox.Inbox.Internals.Storage
{
    internal interface IInboxStorage
    {
        Task<InboxMessageStorage?> GetMessage(Guid hookId, string hookName, TimeProvider timeProvider, TimeSpan lockTimeout);
    }
}
