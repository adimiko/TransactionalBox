using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Extensions;

namespace TransactionalBox.Outbox.Settings.Outbox
{
    public sealed class OutboxSettings
    {
        public Action<IOutboxSerializationConfigurator> ConfigureSerialization { get; set; } = x => x.UseSystemTextJson();

        internal OutboxSettings() { }

        internal void Configure(
            IOutboxSerializationConfigurator serializationConfigurator)
        {
            ConfigureSerialization(serializationConfigurator);
        }
    }
}
