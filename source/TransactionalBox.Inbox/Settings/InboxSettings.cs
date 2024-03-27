using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings
    {
        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator configurator) 
        { 
            ConfigureDeserialization(configurator);
        }
    }
}
