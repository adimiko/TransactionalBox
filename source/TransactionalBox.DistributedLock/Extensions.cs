﻿using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock.Configurators;
using TransactionalBox.DistributedLock.Internals;

namespace TransactionalBox.DistributedLock
{
    public static class Extensions
    {
        public static void AddDistributedLock<T>(
            this IServiceCollection services,
            Action<IDistributedLockStorageConfigurator> storageConfiguration)
            where T : Lock, new()
        {
            var storage = new DistributedLockStorageConfigurator(services);

            storageConfiguration(storage);

            services.AddScoped<IDistributedLock<T>, InternalDistributedLock<T>>();
        }
    }
}
