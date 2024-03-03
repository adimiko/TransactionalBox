using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.OutboxWorker.Configurators
{
    public interface IOutboxWorkerTransportConfigurator
    {
        IServiceCollection Services { get; }
    }
}
