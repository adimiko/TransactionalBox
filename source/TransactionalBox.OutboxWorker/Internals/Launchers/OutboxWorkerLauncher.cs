using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.OutboxWorker.Internals.Jobs;

namespace TransactionalBox.OutboxWorker.Internals.Launchers
{
    internal sealed class OutboxWorkerLauncher : Launcher
    {
        public OutboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IAddMessagesToTransportLauncherSettings addMessagesToTransportSettings,
            ICleanUpProcessedMessagesLauncherSettings cleanUpProcessedMessagesSettings)
            : base(serviceProvider)
        {
            Launch<AddMessagesToTransport>(addMessagesToTransportSettings.NumberOfInstances);
            Launch<CleanUpProcessedMessages>(cleanUpProcessedMessagesSettings.NumberOfInstances);
        }
    }
}
