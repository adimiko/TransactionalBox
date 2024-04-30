namespace TransactionalBox.Outbox.Internals.Jobs
{
    internal interface ICleanUpProcessedOutboxMessagesJobSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }
    }
}
