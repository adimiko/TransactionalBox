namespace TransactionalBox.OutboxWorker.Internals.Jobs
{
    internal interface IAddMessagesToTransportJobSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }

        TimeSpan LockTimeout { get; }
    }
}
