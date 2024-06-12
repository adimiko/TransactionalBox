using Microsoft.EntityFrameworkCore;
using TransactionalBox.EntityFrameworkCore.Internals.InternalPackages.DistributedLock;

namespace TransactionalBox.EntityFrameworkCore.Tests.Internals.InternalPackages.DistributedLock.SeedWork
{
    public sealed class TestDbContext : DbContext
    {
        public TestDbContext() : base() { }

        public TestDbContext(DbContextOptions<TestDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddEntityFrameworkCoreDistributedLock<TestLock>();
        }
    }
}
