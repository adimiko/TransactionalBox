using Microsoft.EntityFrameworkCore;
using TransactionalBox.Loans.Models;

namespace TransactionalBox.Loans.Database
{
    public class LoansDbContext : DbContext
    {
        public DbSet<Loan> Loans { get; set; }

        public LoansDbContext() : base() { }

        public LoansDbContext(DbContextOptions<LoansDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInbox();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LoansDbContext).Assembly);
        }
    }
}
