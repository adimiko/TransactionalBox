using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.OutboxWorker.Configurators
{
    public interface IOutboxWorkerTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
