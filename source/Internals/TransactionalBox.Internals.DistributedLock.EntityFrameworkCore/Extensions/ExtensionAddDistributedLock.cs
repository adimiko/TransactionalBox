﻿using Microsoft.EntityFrameworkCore;
using TransactionalBox.Internals.DistributedLock;
using TransactionalBox.Internals.DistributedLock.EntityFrameworkCore.Internals;

namespace TransactionalBox
{
    internal static class ExtensionAddDistributedLock
    {
        internal static void AddDistributedLock<T>(this ModelBuilder modelBuilder)
            where T : Lock, new()
        {
            modelBuilder.ApplyConfiguration(new LockEntityTypeConfiguration<T>());
        }
    }
}