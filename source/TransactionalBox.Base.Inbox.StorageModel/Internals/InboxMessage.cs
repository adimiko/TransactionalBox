﻿namespace TransactionalBox.Base.Inbox.StorageModel.Internals
{
    public sealed class InboxMessage
    {
        public required Guid Id { get; set; }

        public required DateTime OccurredUtc { get; set; }

        public bool IsProcessed { get; set; }

        public required string Topic { get; set; }

        public required string Payload { get; set; }

        public DateTime? LockUtc { get; set; }

        public string? JobId { get; set; }
    }
}
