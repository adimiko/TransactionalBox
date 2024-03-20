using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.InboxWorker.Internals.Jobs;
using TransactionalBox.InboxWorker.Internals.Settings;

namespace TransactionalBox.InboxWorker.Internals
{
    internal sealed class InboxWorkerLauncher : Launcher
    {
        public InboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IInboxWorkerLauncherSettings settings) 
            : base(serviceProvider)
        {
            Launch<AddMessagesToInboxStorage>(settings.NumberOfAddMessagesToInboxStorageJobExecutors);
        }
    }
}
