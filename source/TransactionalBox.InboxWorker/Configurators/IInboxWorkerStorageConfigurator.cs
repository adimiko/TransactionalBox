using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.InboxWorker.Configurators
{
    public interface IInboxWorkerStorageConfigurator
    {
        IServiceCollection Services { get; }
    }
}
