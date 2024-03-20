using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransactionalBox.InboxBase.StorageModel.EntityFramework.Internals
{
    internal class IdempotentInboxKeyEntityTypeConfiguration : IEntityTypeConfiguration<IdempotentInboxKey>
    {
        public void Configure(EntityTypeBuilder<IdempotentInboxKey> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AddedUtc);
        }
    }
}
