using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using TransactionalBox.Builders;
using TransactionalBox.Internals;
using TransactionalBox.Settings;

namespace TransactionalBox
{
    public static class Extensions
    {
        private const string _packageName = "TransactionalBox";

        public static IServiceCollection AddTransactionalBox(
            this IServiceCollection services,
            Action<ITransactionalBoxBuilder> builderConfiguration,
            Action<TransactionalBoxSettings> settingsConfiguration = null,
            IConfiguration configuration = null)
        {
            var builder = new TransactionalBoxBuilder(services, configuration);
            var settings = new TransactionalBoxSettings();

            builderConfiguration(builder);

            builder
            .Configuration
            .GetSection(_packageName)
            .Bind(settings);

            if (settingsConfiguration != null)
            {
                settingsConfiguration(settings);
            }

            Validator.ValidateObject(settings, new ValidationContext(settings), true);

            services.AddSingleton<IServiceContext>(new ServiceContext(settings.ServiceId, Guid.NewGuid().ToString()));
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton<ISystemClock, SystemClock>();

            return services;
        }
    }
}
