using Microsoft.EntityFrameworkCore;
using TransactionalBox.EntityFrameworkCore.Internals.InternalPackages.DistributedLock;

namespace TransactionalBox.End2EndTests.Internals.InternalPackages.DistributedLockEntityFrameworkCore.SeedWork
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
