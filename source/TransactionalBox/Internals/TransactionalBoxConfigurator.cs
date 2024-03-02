using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators;

namespace TransactionalBox.Internals
{
    internal sealed class TransactionalBoxConfigurator : ITransactionalBoxConfigurator
    {
        public IServiceCollection Services { get; }

        internal TransactionalBoxConfigurator(IServiceCollection services)
        {
            Services = services;
        }
    }
}
