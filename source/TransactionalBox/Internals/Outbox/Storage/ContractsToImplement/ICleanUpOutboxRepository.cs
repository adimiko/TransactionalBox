namespace TransactionalBox.Internals.Outbox.Storage.ContractsToImplement
{
    internal interface ICleanUpOutboxRepository
    {
        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
