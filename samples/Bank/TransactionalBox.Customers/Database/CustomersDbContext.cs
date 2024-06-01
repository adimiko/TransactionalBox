using Microsoft.EntityFrameworkCore;
using TransactionalBox.Customers.Models;

namespace TransactionalBox.Customers.Database
{
    public class CustomersDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomersDbContext() : base() { }

        public CustomersDbContext(DbContextOptions<CustomersDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOutbox();
            modelBuilder.AddInbox();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomersDbContext).Assembly);
        }
    }
}
