namespace TransactionalBox.Internals.Inbox.BackgroundProcesses.CleanUpIdempotencyKeys
{
    internal interface ICleanUpIdempotencyKeysSettings
    {
        int MaxBatchSize { get; }

        TimeSpan Period { get; }
    }
}
