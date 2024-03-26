using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Settings
{
    public sealed class OutboxSettings
    {
        public Action<IOutboxSerializationConfigurator> ConfigureSerialization { get; set; } = x => x.UseSystemTextJson();
    }
}
