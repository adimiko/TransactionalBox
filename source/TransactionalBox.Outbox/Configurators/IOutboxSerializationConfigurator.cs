using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxSerializationConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
