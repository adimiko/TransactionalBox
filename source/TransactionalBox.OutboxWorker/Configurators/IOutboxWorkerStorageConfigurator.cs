using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.OutboxWorker.Configurators
{
    public interface IOutboxWorkerStorageConfigurator
    {
        IServiceCollection Services { get; }
    }
}
