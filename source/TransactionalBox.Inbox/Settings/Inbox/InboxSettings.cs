using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers.Inbox;

namespace TransactionalBox.Inbox.Settings.Inbox
{
    public sealed class InboxSettings : IProcessMessageFromInboxJobSettings, IInboxLauncherSettings
    {
        //TODO to standalone settings ProcessingMessagesFromInbox
        public int NumberOfInstances { get; set; } = 4;

        public TimeSpan DelayWhenInboxIsEmpty { get; set; } = TimeSpan.FromMilliseconds(100);

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator deserializationConfigurator)
        {
            ConfigureDeserialization(deserializationConfigurator);
        }
    }
}
