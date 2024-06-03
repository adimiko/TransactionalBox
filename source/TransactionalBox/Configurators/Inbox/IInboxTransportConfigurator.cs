using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators.Inbox
{
    public interface IInboxTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
