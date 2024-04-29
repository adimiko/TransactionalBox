using TransactionalBox.Inbox.Decompression;

namespace TransactionalBox.Inbox.Internals.Decompression
{
    internal sealed class NoDecompression : IDecompressionAlgorithm
    {
        public Task<byte[]> Decompress(byte[] data) => Task.FromResult(data);
    }
}
