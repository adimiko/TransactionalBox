using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxCompressionAlgorithmConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
