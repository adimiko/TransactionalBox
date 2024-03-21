using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.InboxWorker.Configurators
{
    public interface IInboxWorkerStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
