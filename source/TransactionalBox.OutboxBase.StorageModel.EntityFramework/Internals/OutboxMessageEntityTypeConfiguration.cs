﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionalBox.OutboxBase.StorageModel.Internals;

namespace TransactionalBox.OutboxBase.StorageModel.EntityFramework.Internals
{
    internal sealed class OutboxMessageEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.OccurredUtc).IsRequired();
            builder.Property(x => x.Topic).IsRequired();
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.LockUtc);
            builder.Property(x => x.ProcessedUtc).IsConcurrencyToken();
            builder.Property(x => x.JobId).HasMaxLength(254); //TODO
        }
    }
}
