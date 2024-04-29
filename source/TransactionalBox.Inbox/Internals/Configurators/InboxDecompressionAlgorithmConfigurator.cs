using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;

namespace TransactionalBox.Inbox.Internals.Configurators
{
    internal sealed class InboxDecompressionAlgorithmConfigurator : IInboxDecompressionAlgorithmConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxDecompressionAlgorithmConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
