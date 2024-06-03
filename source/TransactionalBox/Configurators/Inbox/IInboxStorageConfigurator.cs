using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators.Inbox
{
    public interface IInboxStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
