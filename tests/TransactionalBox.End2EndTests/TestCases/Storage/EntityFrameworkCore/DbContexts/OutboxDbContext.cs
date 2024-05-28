using Microsoft.EntityFrameworkCore;

namespace TransactionalBox.End2EndTests.TestCases.Storage.EntityFrameworkCore.DbContexts
{
    internal sealed class OutboxDbContext : DbContext
    {
        public OutboxDbContext() : base() { }

        public OutboxDbContext(DbContextOptions<OutboxDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOutbox();
        }
    }
}
