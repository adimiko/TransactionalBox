using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings
    {
        public AddMessagesToInboxSettings AddMessagesToInboxStorageSettings { get; } = new AddMessagesToInboxSettings();

        public CleanUpInboxSettings CleanUpProcessedInboxMessagesSettings { get; } = new CleanUpInboxSettings();

        public CleanUpIdempotencyKeysSettings CleanUpExpiredIdempotencyKeysSettings { get; } = new CleanUpIdempotencyKeysSettings();

        public Action<IInboxDeserializationConfigurator> ConfigureDeserialization { get; set; } = x => x.UseSystemTextJson();

        public Action<IInboxDecompressionConfigurator> ConfigureDecompression { get; set; } = x => x.UseNoDecompression();

        internal InboxSettings() { }

        internal void Configure(
            IInboxDeserializationConfigurator deserializationConfigurator)
        {
            ConfigureDeserialization(deserializationConfigurator);
        }

        internal void Configure(
            IInboxDecompressionConfigurator decompressionConfigurator)
        {
            ConfigureDecompression(decompressionConfigurator);
        }
    }
}
