using System.IO.Compression;

namespace TransactionalBox.OutboxWorker.Internals.Compression
{
    internal interface IBrotliCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
