using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals.Configurators
{
    internal sealed class InboxDecompressionConfigurator : IInboxDecompressionConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxDecompressionConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
