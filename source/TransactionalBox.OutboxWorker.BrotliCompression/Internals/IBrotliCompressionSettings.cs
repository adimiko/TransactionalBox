using System.IO.Compression;

namespace TransactionalBox.OutboxWorker.BrotliCompression.Internals
{
    internal interface IBrotliCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
