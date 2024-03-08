using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.Internals
{
    internal sealed class OutboxWorkerStorageConfigurator : IOutboxWorkerStorageConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxWorkerStorageConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
