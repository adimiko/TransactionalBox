using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.Outbox.Internals.Jobs;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob;

namespace TransactionalBox.Outbox.Internals.Launchers
{
    internal sealed class OutboxWorkerLauncher : Launcher
    {
        public OutboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IAddMessagesToTransportLauncherSettings addMessagesToTransportSettings,
            ICleanUpProcessedOutboxMessagesLauncherSettings cleanUpProcessedMessagesSettings)
            : base(serviceProvider)
        {
            LaunchJob<AddMessagesToTransport>(addMessagesToTransportSettings.NumberOfInstances);
            LaunchJob<CleanUpProcessedOutboxMessages>(cleanUpProcessedMessagesSettings.NumberOfInstances);
        }
    }
}
