using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.InboxWorker.Configurators
{
    public interface IInboxWorkerDecompressionAlgorithmConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
