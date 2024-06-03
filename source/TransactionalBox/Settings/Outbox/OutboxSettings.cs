using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Outbox;
using TransactionalBox.Internals.Outbox.Configurators;

namespace TransactionalBox.Settings.Outbox
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
