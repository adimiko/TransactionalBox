using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Outbox.Configurators
{
    public interface IOutboxCompressionConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
