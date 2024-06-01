using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.CustomerRegistrations.Models;

namespace TransactionalBox.CustomerRegistrations.Database
{
    public class CustomerRegistrationEntityTypeConfiguration : IEntityTypeConfiguration<CustomerRegistration>
    {
        public void Configure(EntityTypeBuilder<CustomerRegistration> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstName);
            builder.Property(x => x.LastName);
            builder.Property(x => x.Age);
            builder.Property(x => x.IsApproved);
            builder.Property(x => x.CreatedAtUtc);
            builder.Property(x => x.UpdatedAtUtc);
        }
    }
}
