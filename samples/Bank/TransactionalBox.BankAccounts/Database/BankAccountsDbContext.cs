using Microsoft.EntityFrameworkCore;
using TransactionalBox.BankAccounts.Models;

namespace TransactionalBox.BankAccounts.Database
{
    public class BankAccountsDbContext : DbContext
    {
        public DbSet<BankAccount> BankAccounts { get; set; }

        public BankAccountsDbContext() : base() { }

        public BankAccountsDbContext(DbContextOptions<BankAccountsDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInbox();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankAccountsDbContext).Assembly);
        }
    }
}
