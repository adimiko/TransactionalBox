using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
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
