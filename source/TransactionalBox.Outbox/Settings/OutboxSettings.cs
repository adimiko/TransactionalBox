using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Extensions;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class OutboxSettings
    {
        public AddMessagesToTransportSettings AddMessagesToTransportSettings { get; } = new AddMessagesToTransportSettings();

        public CleanUpProcessedOutboxMessagesSettings CleanUpProcessedOutboxMessagesSettings { get; } = new CleanUpProcessedOutboxMessagesSettings();

        public Action<IOutboxSerializationConfigurator> ConfigureSerialization { get; set; } = x => x.UseSystemTextJson();

        public Action<IOutboxCompressionAlgorithmConfigurator> ConfigureCompressionAlgorithm { get; set; } = x => x.UseNoCompression();

        internal OutboxSettings() { }

        internal void Configure(IOutboxSerializationConfigurator serializationConfigurator)
        {
            ConfigureSerialization(serializationConfigurator);
        }

        internal void Configure(IOutboxCompressionAlgorithmConfigurator compressionAlgorithmConfigurator)
        {
            ConfigureCompressionAlgorithm(compressionAlgorithmConfigurator);
        }
    }
}
