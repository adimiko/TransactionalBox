using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Settings
{
    public sealed class OutboxWorkerSettings : IOutboxProcessorSettings, IOutboxOrchestratorSettings
    {
        public int BatchSize { get; set; } = 5000;

        public int NumberOfOutboxProcessor { get; set; } = 2;

        public TimeSpan DelayWhenBatchIsEmpty { get; set; } = TimeSpan.FromSeconds(1);

        public TimeSpan DelayWhenBatchIsNotFull { get; set; } = TimeSpan.FromSeconds(1);

        public TimeSpan LockTimeout { get; set; } = TimeSpan.FromSeconds(15);

        public Action<IOutboxWorkerCompressionAlgorithmConfigurator> ConfigureCompressionAlgorithm { get; set; } = x => x.UseNoCompression();

        internal OutboxWorkerSettings() { }

        internal void Configure(
            IOutboxWorkerCompressionAlgorithmConfigurator compressionAlgorithmConfigurator)
        {
            ConfigureCompressionAlgorithm(compressionAlgorithmConfigurator);
        }
    }
}
