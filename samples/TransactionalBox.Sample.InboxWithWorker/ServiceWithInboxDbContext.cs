using Microsoft.EntityFrameworkCore;
using TransactionalBox.Inbox.Storage.EntityFramework;

namespace TransactionalBox.Sample.InboxWithWorker
{
    public class ServiceWithInboxDbContext : DbContext
    {
        public ServiceWithInboxDbContext() : base() { }

        public ServiceWithInboxDbContext(DbContextOptions<ServiceWithInboxDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInbox();
        }
    }
}
