using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.DistributedLock.Configurators;
using TransactionalBox.DistributedLock.EntityFramework.Internals;
using TransactionalBox.DistributedLock.Internals.Contracts;

namespace TransactionalBox.DistributedLock.EntityFramework
{
    public static class Extensions
    {
        public static IServiceCollection UseEntityFramework<TDbContext>(
            this IDistributedLockStorageConfigurator storageConfigurator)
            where TDbContext : DbContext
        {
            var services = storageConfigurator.Services;

            services.AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>());
            services.AddScoped<IDistributedLockStorage, EntityFrameworkDistributedLockStorage>();

            return services;
        }

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
