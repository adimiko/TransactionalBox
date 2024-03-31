using TransactionalBox.OutboxWorker.Configurators;
using TransactionalBox.OutboxWorker.Internals;

namespace TransactionalBox.OutboxWorker.Settings
{
    public sealed class OutboxWorkerSettings
    {
        public AddMessagesToTransportSettings AddMessagesToTransportSettings { get; } = new AddMessagesToTransportSettings();

        public CleanUpProcessedOutboxMessagesSettings CleanUpProcessedOutboxMessagesSettings { get; } = new CleanUpProcessedOutboxMessagesSettings();

        public Action<IOutboxWorkerCompressionAlgorithmConfigurator> ConfigureCompressionAlgorithm { get; set; } = x => x.UseNoCompression();

        internal OutboxWorkerSettings() { }

        internal void Configure(
            IOutboxWorkerCompressionAlgorithmConfigurator compressionAlgorithmConfigurator)
        {
            ConfigureCompressionAlgorithm(compressionAlgorithmConfigurator);
        }
    }
}
