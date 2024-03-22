using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators
{
    public interface ITransactionalBoxConfigurator
    {
        internal IServiceCollection Services { get; }
    }
}
