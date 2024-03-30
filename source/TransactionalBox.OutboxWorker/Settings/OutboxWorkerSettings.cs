using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;

namespace TransactionalBox.OutboxWorker.Settings
{
    public sealed class OutboxWorkerSettings
    {
        public AddMessagesToTransportSettings AddMessagesToTransportSettings { get; } = new AddMessagesToTransportSettings();

        public Action<IOutboxWorkerCompressionAlgorithmConfigurator> ConfigureCompressionAlgorithm { get; set; } = x => x.UseNoCompression();

        internal OutboxWorkerSettings() { }

        internal void Configure(
            IOutboxWorkerCompressionAlgorithmConfigurator compressionAlgorithmConfigurator)
        {
            ConfigureCompressionAlgorithm(compressionAlgorithmConfigurator);
        }
    }
}
