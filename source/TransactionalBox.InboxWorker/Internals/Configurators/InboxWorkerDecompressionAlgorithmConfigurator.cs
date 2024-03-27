using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.InboxWorker.Configurators;

namespace TransactionalBox.InboxWorker.Internals.Configurators
{
    internal sealed class InboxWorkerDecompressionAlgorithmConfigurator : IInboxWorkerDecompressionAlgorithmConfigurator
    {
        public IServiceCollection Services { get; }

        internal InboxWorkerDecompressionAlgorithmConfigurator(IServiceCollection services) 
        {
            Services = services;
        }
    }
}
