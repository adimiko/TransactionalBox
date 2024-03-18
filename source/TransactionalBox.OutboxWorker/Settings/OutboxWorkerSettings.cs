using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.Settings
{
    public sealed class OutboxWorkerSettings : IOutboxProcessorSettings, IOutboxOrchestratorSettings
    {
        public int BatchSize { get; set; } = 5000;

        public int NumberOfOutboxProcessor { get; set; } = 2;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromSeconds(1);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromSeconds(1);

        public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(15);

        internal OutboxWorkerSettings() { }
    }
}
