using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxStorageConfigurator
    {
        IServiceCollection Services { get; }
    }
}
