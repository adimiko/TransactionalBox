using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals.Configurators
{
    internal sealed class InboxWorkerTransportConfigurator : IInboxWorkerTransportConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxWorkerTransportConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
