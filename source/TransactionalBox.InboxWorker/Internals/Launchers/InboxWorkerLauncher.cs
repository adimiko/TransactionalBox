using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.InboxWorker.Internals.Jobs;

namespace TransactionalBox.InboxWorker.Internals.Launchers
{
    internal sealed class InboxWorkerLauncher : Launcher
    {
        public InboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IAddMessagesToInboxStorageLauncherSettings addMessagesToInboxStorageLauncherSettings,
            ICleanUpProcessedInboxMessagesLauncherSettings cleanUpProcessedInboxMessagesLauncherSettings)
            : base(serviceProvider)
        {
            Launch<AddMessagesToInboxStorage>(addMessagesToInboxStorageLauncherSettings.NumberOfInstances);
            Launch<CleanUpProcessedInboxMessages>(cleanUpProcessedInboxMessagesLauncherSettings.NumberOfInstances);
        }
    }
}
