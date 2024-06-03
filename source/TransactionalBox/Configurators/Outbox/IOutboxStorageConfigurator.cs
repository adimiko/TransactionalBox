using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators.Outbox
{
    public interface IOutboxStorageConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
