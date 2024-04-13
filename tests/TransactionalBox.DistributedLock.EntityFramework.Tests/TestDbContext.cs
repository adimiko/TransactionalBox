﻿using Microsoft.EntityFrameworkCore;

namespace TransactionalBox.DistributedLock.EntityFramework.Tests
{
    public sealed class TestDbContext : DbContext
    {
        public TestDbContext() : base() { }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddDistributedLock<TestLock>();
        }
    }
}
