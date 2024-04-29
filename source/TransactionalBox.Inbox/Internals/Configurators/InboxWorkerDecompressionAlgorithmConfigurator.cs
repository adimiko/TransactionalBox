using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.InboxWorker.Configurators;

namespace TransactionalBox.Inbox.Internals.Configurators
{
    internal sealed class InboxWorkerDecompressionAlgorithmConfigurator : IInboxDecompressionAlgorithmConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxWorkerDecompressionAlgorithmConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
