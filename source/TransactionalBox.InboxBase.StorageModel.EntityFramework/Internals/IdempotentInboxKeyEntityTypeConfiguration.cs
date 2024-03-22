using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.InboxBase.StorageModel.Internals;

namespace TransactionalBox.InboxBase.StorageModel.EntityFramework.Internals
{
    internal sealed class IdempotentInboxKeyEntityTypeConfiguration : IEntityTypeConfiguration<IdempotentInboxKey>
    {
        public void Configure(EntityTypeBuilder<IdempotentInboxKey> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AddedUtc);
        }
    }
}
