using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Settings;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings : IInboxLauncherSettings
    {
        public int NumberOfProcessMessageFromInboxStorageExecutors { get; set; } = 4;

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator configurator) 
        { 
            ConfigureDeserialization(configurator);
        }
    }
}
