﻿using Microsoft.EntityFrameworkCore;
using TransactionalBox.DistributedLock.EntityFramework;
using TransactionalBox.Outbox.Internals.Storage;
using TransactionalBox.Outbox.EntityFrameworkCore.Internals.EntityTypeConfigurations;

namespace TransactionalBox.Outbox
{
    public static class ExtensionAddOutbox
    {
        public static void AddOutbox(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageEntityTypeConfiguration());
            modelBuilder.AddDistributedLock<OutboxDistributedLock>();
        }
    }
}