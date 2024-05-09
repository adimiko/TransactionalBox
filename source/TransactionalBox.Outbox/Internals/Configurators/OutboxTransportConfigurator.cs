using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
{
    internal sealed class OutboxTransportConfigurator : IOutboxTransportConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxTransportConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
