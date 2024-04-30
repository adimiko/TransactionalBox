using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxWorkerStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
