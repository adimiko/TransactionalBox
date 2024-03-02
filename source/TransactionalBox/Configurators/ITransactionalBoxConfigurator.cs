using Microsoft.Extensions.DependencyInjection;

namespace TransactionalBox.Configurators
{
    public interface ITransactionalBoxConfigurator
    {
        IServiceCollection Services { get; }
    }
}
