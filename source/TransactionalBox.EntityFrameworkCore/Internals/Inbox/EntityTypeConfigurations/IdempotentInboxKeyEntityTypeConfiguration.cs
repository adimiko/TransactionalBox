using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.Inbox.Internals.Storage;

namespace TransactionalBox.EntityFrameworkCore.Internals.Inbox.EntityTypeConfigurations
{
    internal sealed class IdempotentInboxKeyEntityTypeConfiguration : IEntityTypeConfiguration<IdempotentInboxKey>
    {
        public void Configure(EntityTypeBuilder<IdempotentInboxKey> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ExpirationUtc);
        }
    }
}
