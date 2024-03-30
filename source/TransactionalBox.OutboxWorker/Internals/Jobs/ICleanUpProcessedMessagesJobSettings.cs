namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal interface ICleanUpProcessedMessagesJobSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }
    }
}
