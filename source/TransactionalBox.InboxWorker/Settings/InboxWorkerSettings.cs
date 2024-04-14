using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals;

namespace TransactionalBox.InboxWorker.Settings
{
    public sealed class InboxWorkerSettings
    {
        public AddMessagesToInboxStorageSettings AddMessagesToInboxStorageSettings { get; } = new AddMessagesToInboxStorageSettings();

        public CleanUpProcessedInboxMessagesSettings CleanUpProcessedInboxMessagesSettings { get; } = new CleanUpProcessedInboxMessagesSettings();

        public CleanUpExpiredIdempotencyKeysSettings CleanUpExpiredIdempotencyKeysSettings { get; } = new CleanUpExpiredIdempotencyKeysSettings();

        public Action<IInboxWorkerDecompressionAlgorithmConfigurator> ConfigureDecompressionAlgorithm { get; set; } = x => x.UseNoDecompression();

        internal InboxWorkerSettings() { }

        internal void Configure(
            IInboxWorkerDecompressionAlgorithmConfigurator decompressionConfigurator)
        {
            ConfigureDecompressionAlgorithm(decompressionConfigurator);
        }
    }
}
