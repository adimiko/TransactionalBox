using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Outbox;

namespace TransactionalBox.Internals.Outbox.Configurators
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
