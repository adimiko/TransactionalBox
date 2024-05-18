﻿using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Configurators;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.EntityFrameworkCore.Internals.EntityTypeConfigurations;
using TransactionalBox.Outbox.EntityFrameworkCore.Internals;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Internals.DistributedLock;
using TransactionalBox.Internals.DistributedLock.EntityFrameworkCore;

namespace TransactionalBox.Outbox
{
    public static class ExtensionUseEntityFramework
    {
        public static void UseEntityFramework<TDbContext>(this IOutboxStorageConfigurator outboxStorageConfigurator)
            where TDbContext : DbContext
        {
            var services = outboxStorageConfigurator.Services;

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());

            services.AddScoped<IEntityFrameworkOutboxUnitOfWork, EntityFrameworkUnitOfWork>();

            services.AddScoped<IOutboxStorage, EntityFrameworkOutboxStorage>();

            services.AddScoped<IAddMessagesToTransportRepository, EntityFrameworkAddMessagesToTransportRepository>();
            services.AddScoped<ICleanUpOutboxRepository, EntityFrameworkCleanUpOutboxRepository>();

            services.AddDistributedLock<OutboxDistributedLock>(x => x.UseEntityFramework());
        }
    }
}
