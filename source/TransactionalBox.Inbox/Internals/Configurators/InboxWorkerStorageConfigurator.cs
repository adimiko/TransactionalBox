using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals.Configurators
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
