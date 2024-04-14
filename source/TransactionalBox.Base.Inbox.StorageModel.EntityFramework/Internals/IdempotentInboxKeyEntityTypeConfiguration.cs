using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.Base.Inbox.StorageModel.Internals;

namespace TransactionalBox.Base.Inbox.StorageModel.EntityFramework.Internals
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
