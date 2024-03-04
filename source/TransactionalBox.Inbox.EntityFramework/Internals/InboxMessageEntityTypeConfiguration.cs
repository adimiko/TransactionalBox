using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TransactionalBox.InboxBase.StorageModel;

namespace TransactionalBox.Inbox.EntityFramework.Internals
{
    internal sealed class InboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<InboxMessage>
    {
        public void Configure(EntityTypeBuilder<InboxMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OccurredUtc);
            builder.Property(x => x.ProcessedUtc);
            builder.Property(x => x.Topic);
            builder.Property(x => x.Payload);
        }
    }
}
