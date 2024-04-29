using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Inbox.Configurators
{
    public interface IInboxTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
