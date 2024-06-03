using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators.Outbox
{
    public interface IOutboxSerializationConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
