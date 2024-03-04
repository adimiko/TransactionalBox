using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.InboxWorker.Configurators
{
    public interface IInboxWorkerTransportConfigurator
    {
        IServiceCollection Services { get; }
    }
}
