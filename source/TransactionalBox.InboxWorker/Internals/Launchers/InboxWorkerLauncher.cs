using TransactionalBox.Base.BackgroundService.Internals.Launchers;
using TransactionalBox.InboxWorker.Internals.Jobs;

namespace TransactionalBox.InboxWorker.Internals.Launchers
{
    internal sealed class InboxWorkerLauncher : Launcher
    {
        public InboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IAddMessagesToInboxStorageLauncherSettings addMessagesToInboxStorageLauncherSettings,
            ICleanUpProcessedInboxMessagesLauncherSettings cleanUpProcessedInboxMessagesLauncherSettings,
            ICleanUpExpiredIdempotencyKeysLauncherSettings cleanUpExpiredIdempotencyKeysLauncherSettings)
            : base(serviceProvider)
        {
            Launch<AddMessagesToInboxStorage>(addMessagesToInboxStorageLauncherSettings.NumberOfInstances);
            Launch<CleanUpProcessedInboxMessages>(cleanUpProcessedInboxMessagesLauncherSettings.NumberOfInstances);
            Launch<CleanUpExpiredIdempotencyKeys>(cleanUpExpiredIdempotencyKeysLauncherSettings.NumberOfInstances);

        }
    }
}
