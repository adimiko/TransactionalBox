using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals.Configurators
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
