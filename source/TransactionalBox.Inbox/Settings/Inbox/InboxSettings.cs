using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers.Inbox;

namespace TransactionalBox.Inbox.Settings.Inbox
{
    public sealed class InboxSettings
    {
        public ProcessingMessagesFromInboxSettings ProcessingMessagesFromInboxSettings { get; } = new ProcessingMessagesFromInboxSettings();

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator deserializationConfigurator)
        {
            ConfigureDeserialization(deserializationConfigurator);
        }
    }
}
