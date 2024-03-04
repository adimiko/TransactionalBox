using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxStorageConfigurator
    {
        IServiceCollection Services { get; }
    }
}
