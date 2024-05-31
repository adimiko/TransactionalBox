using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Configurators;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class OutboxSettings
    {
        public AddMessagesToTransportSettings AddMessagesToTransportSettings { get; } = new AddMessagesToTransportSettings();

        public CleanUpOutboxSettings CleanUpOutboxSettings { get; } = new CleanUpOutboxSettings();

        public Action<IOutboxSerializationConfigurator> ConfigureSerialization { get; set; } = x => x.UseSystemTextJson();

        public Action<IOutboxCompressionConfigurator> ConfigureCompression { get; set; } = x => x.UseNoCompression();

        internal OutboxSettings() { }

        internal void ConfigureDelegates(IServiceCollection services)
        {
            ConfigureSerialization(new OutboxSerializationConfigurator(services));
            ConfigureCompression(new OutboxCompressionConfigurator(services));
        }
    }
}
