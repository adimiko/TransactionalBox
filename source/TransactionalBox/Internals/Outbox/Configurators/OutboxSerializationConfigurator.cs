using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Outbox;

namespace TransactionalBox.Internals.Outbox.Configurators
{
    internal sealed class OutboxSerializationConfigurator : IOutboxSerializationConfigurator
    {
        public IServiceCollection Services { get; }

        public OutboxSerializationConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
