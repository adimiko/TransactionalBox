using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Outbox.Configurators;

namespace TransactionalBox.Outbox.Internals.Configurators
{
    internal sealed class OutboxWorkerCompressionAlgorithmConfigurator : IOutboxWorkerCompressionAlgorithmConfigurator
    {
        public IServiceCollection Services { get; }

        internal OutboxWorkerCompressionAlgorithmConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
