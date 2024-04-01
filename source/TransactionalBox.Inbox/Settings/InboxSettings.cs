using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings : IProcessMessageFromInboxJobSettings, IInboxLauncherSettings
    {
        public int NumberOfInstances { get; set; } = 4;

        public TimeSpan DelayWhenInboxIsEmpty { get; set; } = TimeSpan.FromMilliseconds(100);

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator configurator) 
        { 
            ConfigureDeserialization(configurator);
        }
    }
}
