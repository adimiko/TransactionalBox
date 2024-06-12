namespace TransactionalBox.Internals.InternalPackages.Transport
{
    internal sealed class TransportObject
    {
        internal string Topic { get; init; }

        internal byte[] Payload { get; init; }

        internal string Compression { get; init; }
    }
}
