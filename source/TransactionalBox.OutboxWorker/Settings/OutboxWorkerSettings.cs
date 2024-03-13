using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.Settings
{
    public sealed class OutboxWorkerSettings : IOutboxWorkerSettings
    {
        public int BatchSize { get; set; } = 5000;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromSeconds(1);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromSeconds(1);

        public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(15);

        internal OutboxWorkerSettings() { }
    }
}
