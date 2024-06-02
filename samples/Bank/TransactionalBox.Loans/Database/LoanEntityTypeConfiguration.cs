using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.Loans.Models;

namespace TransactionalBox.Loans.Database
{
    public class LoanEntityTypeConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CustomerId);
            builder.Property(x => x.Amount);
        }
    }
}
