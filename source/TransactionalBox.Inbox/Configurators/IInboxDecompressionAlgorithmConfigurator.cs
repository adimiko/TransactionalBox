using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxDecompressionAlgorithmConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
