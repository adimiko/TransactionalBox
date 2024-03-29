﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock.Configurators;
using TransactionalBox.DistributedLock.EntityFramework.Internals;
using TransactionalBox.DistributedLock.Internals;

namespace TransactionalBox.DistributedLock.EntityFramework
{
    public static class Extensions
    {
        public static IServiceCollection UseEntityFramework(
            this IDistributedLockStorageConfigurator storageConfigurator)
        {
            var services = storageConfigurator.Services;

            services.AddScoped<IDistributedLockStorage, EntityFrameworkDistributedLockStorage>();

            return services;
        }

        public static void AddDistributedLock<T>(this ModelBuilder modelBuilder) 
            where T : Lock, new()
        {
            modelBuilder.ApplyConfiguration(new LockEntityTypeConfiguration<T>());
        }

    }
}
