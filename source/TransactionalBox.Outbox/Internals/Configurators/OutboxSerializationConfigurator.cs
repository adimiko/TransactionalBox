using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
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
