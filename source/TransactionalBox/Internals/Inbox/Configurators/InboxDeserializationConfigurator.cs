using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators.Inbox;

namespace TransactionalBox.Internals.Inbox.Configurators
{
    internal sealed class InboxDeserializationConfigurator : IInboxDeserializationConfigurator
    {
        public IServiceCollection Services { get; }

        public InboxDeserializationConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
