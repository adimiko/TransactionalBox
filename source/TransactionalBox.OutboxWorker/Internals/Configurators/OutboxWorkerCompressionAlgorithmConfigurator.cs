using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.OutboxWorker.Configurators;

namespace TransactionalBox.OutboxWorker.Internals.Configurators
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
