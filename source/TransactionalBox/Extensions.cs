﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Configurators;
using TransactionalBox.Internals;
using TransactionalBox.Settings;

namespace TransactionalBox
{
    public static class Extensions
    {
        public static IServiceCollection AddTransactionalBox(
            this IServiceCollection services,
            Action<ITransactionalBoxConfigurator> configuration,
            Action<TransactionalBoxSettings> settingsConfiguration)
        {
            var configuratior = new TransactionalBoxConfigurator(services);
            var settings = new TransactionalBoxSettings();

            configuration(configuratior);

            settingsConfiguration(settings);

            services.AddSingleton<IServiceContext>(new ServiceContext(settings.ServiceId, Guid.NewGuid().ToString()));
            services.AddSingleton(TimeProvider.System);
            services.AddSingleton<ISystemClock, SystemClock>();


            return services;
        }
    }
}
