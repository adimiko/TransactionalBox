using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.InboxWorker.Configurators
{
    public interface IInboxWorkerTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
