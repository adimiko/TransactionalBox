using System.IO.Compression;

namespace TransactionalBox.Outbox.Internals.Compression.Brotli
{
    internal interface IBrotliCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
