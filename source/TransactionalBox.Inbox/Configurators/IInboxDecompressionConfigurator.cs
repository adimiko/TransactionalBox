using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxDecompressionConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
