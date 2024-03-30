using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.InboxWorker.Internals.Jobs;

namespace TransactionalBox.InboxWorker.Internals.Launchers
{
    internal sealed class InboxWorkerLauncher : Launcher
    {
        public InboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IAddMessagesToInboxStorageLauncherSettings settings)
            : base(serviceProvider)
        {
            Launch<AddMessagesToInboxStorage>(settings.NumberOfInstances);
        }
    }
}
