namespace TransactionalBox.Internals.Inbox.Storage.ContractsToImplement
{
    internal interface ICleanUpIdempotencyKeysRepository
    {
        Task<int> RemoveExpiredIdempotencyKeys(int batchSize, DateTime nowUtc);
    }
}
