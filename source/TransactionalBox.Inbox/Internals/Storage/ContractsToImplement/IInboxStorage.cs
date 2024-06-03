namespace TransactionalBox.Inbox.Internals.Storage.ContractsToImplement
{
    internal interface IInboxStorage
    {
        Task<InboxMessageStorage?> GetMessage(Guid hookId, string hookName, TimeProvider timeProvider, TimeSpan lockTimeout);
    }
}
