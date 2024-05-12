namespace TransactionalBox.Outbox.Internals.Compression.NoCompression
{
    internal sealed class NoCompression : ICompressionAlgorithm
    {
        public Task<byte[]> Compress(byte[] data) => Task.FromResult(data);
    }
}
