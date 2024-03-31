namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal interface ICleanUpProcessedOutboxMessagesJobSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }
    }
}
