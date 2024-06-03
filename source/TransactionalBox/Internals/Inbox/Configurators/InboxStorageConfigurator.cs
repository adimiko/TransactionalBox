using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Inbox;

namespace TransactionalBox.Internals.Inbox.Configurators
{
    internal sealed class InboxStorageConfigurator : IInboxStorageConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxStorageConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
