namespace TransactionalBox.OutboxWorker.Internals.Contracts
{
    internal interface IOutboxProcessorSettings
    {
        int BatchSize { get; }

        TimeSpan DelayWhenBatchIsEmpty { get; }

        TimeSpan DelayWhenBatchIsNotFull { get; }

        TimeSpan LockTimeout { get; }
    }
}
