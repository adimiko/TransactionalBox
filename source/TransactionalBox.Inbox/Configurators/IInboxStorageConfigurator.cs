using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
