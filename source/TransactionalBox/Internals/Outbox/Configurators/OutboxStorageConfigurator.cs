using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Outbox;

namespace TransactionalBox.Internals.Outbox.Configurators
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
