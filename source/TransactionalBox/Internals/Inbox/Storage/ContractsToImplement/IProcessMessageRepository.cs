namespace TransactionalBox.Internals.Inbox.Storage.ContractsToImplement
{
    internal interface IProcessMessageRepository
    {
        Task<InboxMessageStorage?> GetMessage(Guid hookId, string hookName, TimeProvider timeProvider, TimeSpan lockTimeout);
    }
}
