using TransactionalBox.OutboxWorker.Compression;

namespace TransactionalBox.OutboxWorker.Internals.Compression
{
    internal sealed class NoCompression : ICompressionAlgorithm
    {
        public Task<byte[]> Compress(byte[] data) => Task.FromResult(data);
    }
}
