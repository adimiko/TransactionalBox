using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
