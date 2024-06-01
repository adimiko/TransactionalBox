using Microsoft.EntityFrameworkCore;
using TransactionalBox.CustomerRegistrations.Models;

namespace TransactionalBox.CustomerRegistrations.Database
{
    public sealed class CustomerRegistrationDbContext : DbContext
    {
        public DbSet<CustomerRegistration> CustomerRegistrations { get; set; }

        public CustomerRegistrationDbContext() : base() { }

        public CustomerRegistrationDbContext(DbContextOptions<CustomerRegistrationDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddOutbox();
        }
    }
}
