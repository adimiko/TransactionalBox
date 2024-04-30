using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxWorkerCompressionAlgorithmConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
