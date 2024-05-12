using System.IO.Compression;

namespace TransactionalBox.Outbox.Internals.Compression.GZip
{
    internal interface IGZipCompressionSettings
    {
        CompressionLevel CompressionLevel { get; }
    }
}
