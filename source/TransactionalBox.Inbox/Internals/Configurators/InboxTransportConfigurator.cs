using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals.Configurators
{
    internal sealed class InboxTransportConfigurator : IInboxTransportConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxTransportConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
