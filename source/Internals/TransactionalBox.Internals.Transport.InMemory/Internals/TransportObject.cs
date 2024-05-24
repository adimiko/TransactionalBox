namespace TransactionalBox.Internals.Transport.InMemory.Internals
{
    internal sealed class TransportObject
    {
        internal string Topic { get; init; }

        internal byte[] Payload { get; init; }

        internal string ContentType { get; init; }
    }
}
