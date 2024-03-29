﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TransactionalBox.Inbox.Configurators;
using TransactionalBox.Inbox.EntityFramework.Internals;
using TransactionalBox.Inbox.Internals;
using TransactionalBox.InboxBase.StorageModel.EntityFramework.Internals;

namespace TransactionalBox.Inbox.EntityFramework
{
    public static class Extensions
    {
        public static void UseEntityFramework<TDbContext>(this IInboxStorageConfigurator storageConfigurator)
            where TDbContext : DbContext
        {
            var services = storageConfigurator.Services;

            services.AddScoped<DbContext>(x => x.GetRequiredService<TDbContext>());
            services.AddScoped<IInboxStorage, InboxStorage>();
        }

        public static void AddInbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.AddInboxStorageModel();
        }
    }
}
