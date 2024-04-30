using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.Internals;

namespace TransactionalBox.Inbox.Settings.InboxWorker
{
    public sealed class InboxWorkerSettings
    {
        public AddMessagesToInboxStorageSettings AddMessagesToInboxStorageSettings { get; } = new AddMessagesToInboxStorageSettings();

        public CleanUpProcessedInboxMessagesSettings CleanUpProcessedInboxMessagesSettings { get; } = new CleanUpProcessedInboxMessagesSettings();

        public CleanUpExpiredIdempotencyKeysSettings CleanUpExpiredIdempotencyKeysSettings { get; } = new CleanUpExpiredIdempotencyKeysSettings();

        public Action<IInboxDecompressionAlgorithmConfigurator> ConfigureDecompressionAlgorithm { get; set; } = x => x.UseNoDecompression();

        internal InboxWorkerSettings() { }

        internal void Configure(
            IInboxDecompressionAlgorithmConfigurator decompressionConfigurator)
        {
            ConfigureDecompressionAlgorithm(decompressionConfigurator);
        }
    }
}
