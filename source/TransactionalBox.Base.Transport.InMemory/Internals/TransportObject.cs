namespace TransactionalBox.Base.Transport.InMemory.Internals
{
    internal sealed class TransportObject
    {
        internal string Topic { get; init; }

        internal byte[] Payload { get; init; }
    }
}
