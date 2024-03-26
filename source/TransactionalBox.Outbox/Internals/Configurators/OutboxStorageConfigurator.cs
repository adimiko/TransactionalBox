using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
{
    internal sealed class OutboxStorageConfigurator : IOutboxStorageConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxStorageConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
