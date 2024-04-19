using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.OutboxWorker.Internals.Jobs;
using TransactionalBox.OutboxWorker.Internals.Jobs.AddMessagesToTransportJob;

namespace TransactionalBox.OutboxWorker.Internals.Launchers
{
    internal sealed class OutboxWorkerLauncher : Launcher
    {
        public OutboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IAddMessagesToTransportLauncherSettings addMessagesToTransportSettings,
            ICleanUpProcessedOutboxMessagesLauncherSettings cleanUpProcessedMessagesSettings)
            : base(serviceProvider)
        {
            Launch<AddMessagesToTransport>(addMessagesToTransportSettings.NumberOfInstances);
            Launch<CleanUpProcessedOutboxMessages>(cleanUpProcessedMessagesSettings.NumberOfInstances);
        }
    }
}
