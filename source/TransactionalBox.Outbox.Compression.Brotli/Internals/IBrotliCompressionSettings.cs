using System.IO.Compression;

namespace TransactionalBox.Outbox.Compression.Brotli.Internals
{
    internal interface IBrotliCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
