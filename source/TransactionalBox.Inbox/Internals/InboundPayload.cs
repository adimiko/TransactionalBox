namespace TransactionalBox.Inbox.Internals
{
    public sealed class InboundPayload
    {
        public Metadata Metadata { get; internal init; }

        public dynamic Message { get; internal init; }

    }
}
