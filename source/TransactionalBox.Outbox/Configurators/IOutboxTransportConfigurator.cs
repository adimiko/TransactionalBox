using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
