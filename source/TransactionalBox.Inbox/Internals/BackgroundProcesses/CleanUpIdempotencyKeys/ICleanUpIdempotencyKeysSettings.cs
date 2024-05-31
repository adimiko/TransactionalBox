namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys
{
    internal interface ICleanUpIdempotencyKeysSettings
    {
        int MaxBatchSize { get; }

        TimeSpan Period { get; }
    }
}
