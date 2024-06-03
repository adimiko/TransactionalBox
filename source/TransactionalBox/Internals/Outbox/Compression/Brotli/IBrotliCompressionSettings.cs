using System.IO.Compression;

namespace TransactionalBox.Internals.Outbox.Compression.Brotli
{
    internal interface IBrotliCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
