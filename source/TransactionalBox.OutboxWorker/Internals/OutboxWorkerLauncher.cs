using TransactionalBox.BackgroundServiceBase.Internals;
using TransactionalBox.OutboxWorker.Internals.Contracts;
using TransactionalBox.OutboxWorker.Internals.Jobs;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxWorkerLauncher : Launcher
    {
        public OutboxWorkerLauncher(
            IServiceProvider serviceProvider,
            IOutboxOrchestratorSettings settings) 
            : base(serviceProvider)
        {
            Launch<AddMessagesToTransport>(settings.NumberOfAddMessagesToTransportJobExecutors);
        }
    }
}
