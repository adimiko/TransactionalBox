using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings
    {
        public AddMessagesToInboxStorageSettings AddMessagesToInboxStorageSettings { get; } = new AddMessagesToInboxStorageSettings();

        public CleanUpProcessedInboxMessagesSettings CleanUpProcessedInboxMessagesSettings { get; } = new CleanUpProcessedInboxMessagesSettings();

        public CleanUpExpiredIdempotencyKeysSettings CleanUpExpiredIdempotencyKeysSettings { get; } = new CleanUpExpiredIdempotencyKeysSettings();

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        public Action<IInboxDecompressionAlgorithmConfigurator> ConfigureDecompressionAlgorithm { get; set; } = x => x.UseNoDecompression();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator deserializationConfigurator)
        {
            ConfigureDeserialization(deserializationConfigurator);
        }

        internal void Configure(
            IInboxDecompressionAlgorithmConfigurator decompressionConfigurator)
        {
            ConfigureDecompressionAlgorithm(decompressionConfigurator);
        }
    }
}
