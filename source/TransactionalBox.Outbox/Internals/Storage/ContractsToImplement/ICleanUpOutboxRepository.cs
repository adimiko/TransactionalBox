namespace TransactionalBox.Outbox.Internals.Storage.ContractsToImplement
{
    internal interface ICleanUpOutboxRepository
    {
        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
