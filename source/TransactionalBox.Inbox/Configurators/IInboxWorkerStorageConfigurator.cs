using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxWorkerStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
