using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
{
    internal sealed class OutboxCompressionAlgorithmConfigurator : IOutboxCompressionAlgorithmConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxCompressionAlgorithmConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
