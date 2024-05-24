namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal sealed class NoDecompression : IDecompression
    {
        public string Name { get; } = "no_compression";

        public Task<byte[]> Decompress(byte[] data) => Task.FromResult(data);
    }
}
