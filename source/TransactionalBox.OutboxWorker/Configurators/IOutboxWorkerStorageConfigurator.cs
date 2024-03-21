using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.OutboxWorker.Configurators
{
    public interface IOutboxWorkerStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
