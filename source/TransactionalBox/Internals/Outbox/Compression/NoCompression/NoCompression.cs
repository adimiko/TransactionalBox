namespace TransactionalBox.Internals.Outbox.Compression.NoCompression
{
    internal sealed class NoCompression : ICompression
    {
        public string Name { get; } = "no_compression";

        public Task<byte[]> Compress(byte[] data) => Task.FromResult(data);
    }
}
