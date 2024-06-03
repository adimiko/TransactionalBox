using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators.Outbox
{
    public interface IOutboxCompressionConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
