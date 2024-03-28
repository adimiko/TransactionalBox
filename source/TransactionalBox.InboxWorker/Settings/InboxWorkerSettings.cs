using TransactionalBox.InboxWorker.Configurators;
using TransactionalBox.InboxWorker.Internals;
using TransactionalBox.InboxWorker.Internals.Settings;

namespace TransactionalBox.InboxWorker.Settings
{
    public sealed class InboxWorkerSettings : IInboxWorkerLauncherSettings
    {
        public int NumberOfAddMessagesToInboxStorageJobExecutors { get; set; } = 1;

        public Action<IInboxWorkerDecompressionAlgorithmConfigurator> ConfigureDecompressionAlgorithm { get; set; } = x => x.UseNoDecompression();

        internal InboxWorkerSettings() { }

        internal void Configure(
            IInboxWorkerDecompressionAlgorithmConfigurator decompressionConfigurator)
        {
            ConfigureDecompressionAlgorithm(decompressionConfigurator);
        }
    }
}
