using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Extensions;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class OutboxSettings
    {
        public AddMessagesToTransportSettings AddMessagesToTransportSettings { get; } = new AddMessagesToTransportSettings();

        public CleanUpOutboxSettings CleanUpProcessedOutboxMessagesSettings { get; } = new CleanUpOutboxSettings();

        public Action<IOutboxSerializationConfigurator> ConfigureSerialization { get; set; } = x => x.UseSystemTextJson();

        public Action<IOutboxCompressionConfigurator> ConfigureCompression { get; set; } = x => x.UseNoCompression();

        internal OutboxSettings() { }

        internal void Configure(IOutboxSerializationConfigurator serializationConfigurator)
        {
            ConfigureSerialization(serializationConfigurator);
        }

        internal void Configure(IOutboxCompressionConfigurator compressionConfigurator)
        {
            ConfigureCompression(compressionConfigurator);
        }
    }
}
