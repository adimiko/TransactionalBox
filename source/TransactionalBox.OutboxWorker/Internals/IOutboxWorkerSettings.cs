namespace TransactionalBox.OutboxWorker.Internals
{
    internal interface IOutboxWorkerSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }

        TimeSpan LockTimeout { get; }
    }
}
