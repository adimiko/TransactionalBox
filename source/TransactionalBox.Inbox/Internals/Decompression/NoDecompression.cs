namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal sealed class NoDecompression : IDecompression
    {
        public Task<byte[]> Decompress(byte[] data) => Task.FromResult(data);
    }
}
