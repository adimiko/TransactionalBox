using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
{
    internal sealed class OutboxWorkerTransportConfigurator : IOutboxWorkerTransportConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxWorkerTransportConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
