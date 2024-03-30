using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.OutboxWorker.Internals.Jobs;

namespace TransactionalBox.OutboxWorker.Internals.Launchers
{
    internal sealed class OutboxWorkerLauncher : Launcher
    {
        public OutboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IAddMessagesToTransportLauncherSettings settings)
            : base(serviceProvider)
        {
            Launch<AddMessagesToTransport>(settings.NumberOfInstances);
        }
    }
}
