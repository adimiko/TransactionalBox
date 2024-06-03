using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Outbox;

namespace TransactionalBox.Internals.Outbox.Configurators
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
