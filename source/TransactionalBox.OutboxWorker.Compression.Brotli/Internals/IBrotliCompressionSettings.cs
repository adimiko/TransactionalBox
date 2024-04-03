using System.IO.Compression;

namespace TransactionalBox.OutboxWorker.Compression.Brotli.Internals
{
    internal interface IBrotliCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
