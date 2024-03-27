using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.OutboxWorker.Configurators
{
    public interface IOutboxWorkerCompressionAlgorithmConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
