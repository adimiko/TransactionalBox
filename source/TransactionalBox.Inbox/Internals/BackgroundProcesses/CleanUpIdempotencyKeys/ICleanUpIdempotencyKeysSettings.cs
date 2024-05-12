namespace TransactionalBox.Inbox.Internals.BackgroundProcesses.CleanUpIdempotencyKeys
{
    internal interface ICleanUpIdempotencyKeysSettings
    {
        int BatchSize { get; }

        TimeSpan Period { get; }
    }
}
