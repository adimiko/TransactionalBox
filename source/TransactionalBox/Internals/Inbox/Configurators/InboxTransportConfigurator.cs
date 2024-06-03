using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Inbox;

namespace TransactionalBox.Internals.Inbox.Configurators
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
