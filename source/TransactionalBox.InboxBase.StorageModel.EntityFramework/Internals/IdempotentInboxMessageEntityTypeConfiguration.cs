using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TransactionalBox.InboxBase.StorageModel.EntityFramework.Internals
{
    internal class IdempotentInboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<IdempotentInboxMessage>
    {
        public void Configure(EntityTypeBuilder<IdempotentInboxMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AddedUtc);
        }
    }
}
