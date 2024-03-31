﻿namespace TransactionalBox.Inbox.Internals
{
    public sealed class Metadata
    {
        public string Source { get; init; }

        public DateTime OccurredUtc { get; init; }

        public string CorrelationId { get; init; }
    }
}
