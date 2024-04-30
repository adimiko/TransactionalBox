using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxWorkerTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
