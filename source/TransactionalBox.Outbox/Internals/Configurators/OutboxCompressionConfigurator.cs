using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
{
    internal sealed class OutboxCompressionConfigurator : IOutboxCompressionConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxCompressionConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
