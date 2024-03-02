using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using TransactionalBox.Outbox.EntityFramework;

namespace TransactionalBox.Sample.WebApi
{
    public class SampleDbContext : DbContext
    {
        public SampleDbContext() : base() { }

        public SampleDbContext(DbContextOptions<SampleDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOutbox();
        }
    }
}
