namespace TransactionalBox.Internals.Inbox.Storage.ContractsToImplement
{
    internal interface ICleanUpInboxRepository
    {
        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
