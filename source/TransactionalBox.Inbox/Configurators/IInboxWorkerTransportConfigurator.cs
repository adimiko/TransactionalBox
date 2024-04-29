using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxWorkerTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
