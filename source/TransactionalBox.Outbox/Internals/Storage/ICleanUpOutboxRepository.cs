namespace TransactionalBox.Outbox.Internals.Storage
{
    internal interface ICleanUpOutboxRepository
    {
        Task<int> RemoveProcessedMessages(int batchSize);
    }
}
