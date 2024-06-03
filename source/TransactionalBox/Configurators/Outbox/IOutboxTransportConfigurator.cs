using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators.Outbox
{
    public interface IOutboxTransportConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
