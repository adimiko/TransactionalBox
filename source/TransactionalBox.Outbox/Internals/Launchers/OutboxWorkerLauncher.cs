using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.Outbox.Internals.Jobs;
using TransactionalBox.Outbox.Internals.Jobs.AddMessagesToTransportJob;
using TransactionalBox.Outbox.Internals.Launchers.Settings;

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
            //Launch<AddMessagesToTransport>(addMessagesToTransportSettings.NumberOfInstances); //TODO
            Launch<CleanUpProcessedOutboxMessages>(cleanUpProcessedMessagesSettings.NumberOfInstances);
        }
    }
}
