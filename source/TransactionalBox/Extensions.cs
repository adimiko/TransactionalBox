using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Builders;
using TransactionalBox.Internals;
using TransactionalBox.Settings;

namespace TransactionalBox
{
    public static class Extensions
    {
        public static IServiceCollection AddTransactionalBox(
            this IServiceCollection services,
            Action<ITransactionalBoxBuilder> builder,
            Action<TransactionalBoxSettings> settingsConfiguration)
        {
            var configuratior = new TransactionalBoxBuilder(services);
            var settings = new TransactionalBoxSettings();

            builder(configuratior);

            settingsConfiguration(settings);

            services.AddSingleton<IServiceContext>(new ServiceContext(settings.ServiceId, Guid.NewGuid().ToString()));
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton<ISystemClock, SystemClock>();


            return services;
        }
    }
}
