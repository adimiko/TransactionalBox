namespace TransactionalBox.Outbox.Internals.Compression.NoCompression
{
    internal sealed class NoCompression : ICompression
    {
        public Task<byte[]> Compress(byte[] data) => Task.FromResult(data);
    }
}
