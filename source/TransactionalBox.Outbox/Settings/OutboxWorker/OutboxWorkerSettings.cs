using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals;

namespace TransactionalBox.Outbox.Settings.OutboxWorker
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
