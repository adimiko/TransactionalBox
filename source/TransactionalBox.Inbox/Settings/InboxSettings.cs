using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals.Extensions;

namespace TransactionalBox.Inbox.Settings
{
    public sealed class InboxSettings
    {
        public AddMessagesToInboxSettings AddMessagesToInboxSettings { get; } = new AddMessagesToInboxSettings();

        public CleanUpInboxSettings CleanUpInboxSettings { get; } = new CleanUpInboxSettings();

        public CleanUpIdempotencyKeysSettings CleanUpIdempotencyKeysSettings { get; } = new CleanUpIdempotencyKeysSettings();

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
