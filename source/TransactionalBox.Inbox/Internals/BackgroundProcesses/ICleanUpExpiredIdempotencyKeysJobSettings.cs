namespace TransactionalBox.Inbox.Internals.BackgroundProcesses
{
    internal interface ICleanUpExpiredIdempotencyKeysJobSettings
    {
        int BatchSize { get; }

        TimeSpan Period { get; }
    }
}
