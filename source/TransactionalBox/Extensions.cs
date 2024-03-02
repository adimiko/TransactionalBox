using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators;
using TransactionalBox.Internals;

namespace TransactionalBox
{
    public static class Extensions
    {
        public static IServiceCollection AddTransactionalBox(
            this IServiceCollection services,
            Action<ITransactionalBoxConfigurator> configure)
        {
            var configuratior = new TransactionalBoxConfigurator(services);

            configure(configuratior);

            return services;
        }
    }
}
