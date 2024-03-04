using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;

namespace TransactionalBox.InboxWorker.Internals
{
    internal sealed class InboxWorkerStorageConfigurator : IInboxWorkerStorageConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxWorkerStorageConfigurator(IServiceCollection services) 
        {
            Services = services;
        }
    }
}
