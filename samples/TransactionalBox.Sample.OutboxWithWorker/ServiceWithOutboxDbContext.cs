using Microsoft.EntityFrameworkCore;
using TransactionalBox.Outbox.Storage.EntityFramework;

namespace TransactionalBox.Sample.OutboxWithWorker
{
    public class ServiceWithOutboxDbContext : DbContext
    {
        public ServiceWithOutboxDbContext() : base() { }

        public ServiceWithOutboxDbContext(DbContextOptions<ServiceWithOutboxDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOutbox();
        }
    }
}
