using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.Inbox.Internals.Jobs;
using TransactionalBox.Inbox.Internals.Launchers;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings : IProcessMessageFromInboxJobSettings, IInboxLauncherSettings
    {
        //TODO to standalone settings ProcessingMessagesFromInbox
        public int NumberOfInstances { get; set; } = 4;

        public TimeSpan DelayWhenInboxIsEmpty { get; set; } = TimeSpan.FromMilliseconds(100);

        public AddMessagesToInboxStorageSettings AddMessagesToInboxStorageSettings { get; } = new AddMessagesToInboxStorageSettings();

        public CleanUpProcessedInboxMessagesSettings CleanUpProcessedInboxMessagesSettings { get; } = new CleanUpProcessedInboxMessagesSettings();

        public CleanUpExpiredIdempotencyKeysSettings CleanUpExpiredIdempotencyKeysSettings { get; } = new CleanUpExpiredIdempotencyKeysSettings();

        public Action<IInboxDecompressionAlgorithmConfigurator> ConfigureDecompressionAlgorithm { get; set; } = x => x.UseNoDecompression();

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator deserializationConfigurator,
            IInboxDecompressionAlgorithmConfigurator decompressionConfigurator) 
        { 
            ConfigureDeserialization(deserializationConfigurator);
            ConfigureDecompressionAlgorithm(decompressionConfigurator);
        }
    }
}
