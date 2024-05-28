using Microsoft.EntityFrameworkCore;

namespace TransactionalBox.End2EndTests.TestCases.EntityFramework.DbContexts
{
    internal class InboxDbContext : DbContext
    {
        public InboxDbContext() : base() { }

        public InboxDbContext(DbContextOptions<InboxDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInbox();
        }
    }
}
