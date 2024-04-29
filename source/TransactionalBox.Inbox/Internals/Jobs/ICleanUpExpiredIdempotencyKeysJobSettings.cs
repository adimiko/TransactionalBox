namespace TransactionalBox.Inbox.Internals.Jobs
{
    internal interface ICleanUpExpiredIdempotencyKeysJobSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }
    }
}
